import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import AssignmentIndOutlinedIcon from '@mui/icons-material/AssignmentIndOutlined';
import InboxOutlinedIcon from '@mui/icons-material/InboxOutlined';
import ConfirmationNumberOutlinedIcon from '@mui/icons-material/ConfirmationNumberOutlined';
import DescriptionOutlinedIcon from '@mui/icons-material/DescriptionOutlined';
import FlagOutlinedIcon from '@mui/icons-material/FlagOutlined';
import PendingActionsOutlinedIcon from '@mui/icons-material/PendingActionsOutlined';
import PersonOutlinedIcon from '@mui/icons-material/PersonOutlined';
import SearchOffIcon from '@mui/icons-material/SearchOff';
import type { ReactNode } from 'react';
import Alert from '@mui/material/Alert';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Container from '@mui/material/Container';
import Divider from '@mui/material/Divider';
import FormControl from '@mui/material/FormControl';
import FormHelperText from '@mui/material/FormHelperText';
import InputLabel from '@mui/material/InputLabel';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import MenuItem from '@mui/material/MenuItem';
import Paper from '@mui/material/Paper';
import Select from '@mui/material/Select';
import TextField from '@mui/material/TextField';
import ToggleButton from '@mui/material/ToggleButton';
import Typography from '@mui/material/Typography';
import { type FormEvent, useCallback, useEffect, useState } from 'react';
import { Link as RouterLink, useParams } from 'react-router-dom';
import { ApiRequestError } from '../api/client';
import {
  addComment,
  changeTicketStatus,
  getTicket,
  updateTicket,
} from '../api/ticketsApi';
import { getUsers } from '../api/usersApi';
import { ErrorBanner } from '../components/ErrorBanner';
import { LoadingState } from '../components/LoadingState';
import { PageHeader } from '../components/layout/PageHeader';
import { PriorityChip } from '../components/PriorityChip';
import { TicketStatusChip } from '../components/TicketStatusChip';
import { UserAvatar } from '../components/UserAvatar';
import type {
  TicketDetail,
  TicketPriority,
  TicketStatus,
  User,
} from '../types/api';
import {
  getStoredCreatedBy,
  setStoredCreatedBy,
} from '../utils/createdByStorage';
import { formatDate } from '../utils/formatDate';
import { formatStatusLabel } from '../utils/labels';

const PRIORITY_OPTIONS: TicketPriority[] = ['Low', 'Medium', 'High'];
const UNASSIGNED = '';
const STATUS_PLACEHOLDER = '';

function FormSection({
  icon,
  title,
  children,
}: {
  icon: ReactNode;
  title: string;
  children: ReactNode;
}) {
  return (
    <Box>
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
        <Box sx={{ color: 'primary.main', display: 'flex' }}>{icon}</Box>
        <Typography variant="subtitle1">{title}</Typography>
      </Box>
      {children}
    </Box>
  );
}

function parseTicketId(idParam: string | undefined): number | null {
  if (!idParam) {
    return null;
  }
  const parsed = Number(idParam);
  if (!Number.isInteger(parsed) || parsed <= 0) {
    return null;
  }
  return parsed;
}

function syncFormFromTicket(ticket: TicketDetail) {
  return {
    title: ticket.title,
    description: ticket.description ?? '',
    priority: ticket.priority,
    assignedTo: ticket.assignedTo ? String(ticket.assignedTo) : UNASSIGNED,
  };
}

export function TicketDetailPage() {
  const { id: idParam } = useParams<{ id: string }>();
  const ticketId = parseTicketId(idParam);

  const [ticket, setTicket] = useState<TicketDetail | null>(null);
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [notFound, setNotFound] = useState(false);
  const [loadError, setLoadError] = useState<string | null>(null);
  const [reloadKey, setReloadKey] = useState(0);

  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [priority, setPriority] = useState<TicketPriority>('Medium');
  const [assignedTo, setAssignedTo] = useState<string>(UNASSIGNED);
  const [saving, setSaving] = useState(false);
  const [saveError, setSaveError] = useState<string | null>(null);

  const [statusSelection, setStatusSelection] = useState<string>(STATUS_PLACEHOLDER);
  const [patchingStatus, setPatchingStatus] = useState(false);
  const [statusError, setStatusError] = useState<string | null>(null);

  const [commentMessage, setCommentMessage] = useState('');
  const [commentCreatedBy, setCommentCreatedBy] = useState('');
  const [submittingComment, setSubmittingComment] = useState(false);
  const [commentError, setCommentError] = useState<string | null>(null);

  const loadTicket = useCallback(async () => {
    if (ticketId === null) {
      return;
    }

    setLoading(true);
    setNotFound(false);
    setLoadError(null);

    try {
      const data = await getTicket(ticketId);
      setTicket(data);
      const form = syncFormFromTicket(data);
      setTitle(form.title);
      setDescription(form.description);
      setPriority(form.priority);
      setAssignedTo(form.assignedTo);
      setStatusSelection(STATUS_PLACEHOLDER);
      setStatusError(null);
      setSaveError(null);
    } catch (err) {
      if (err instanceof ApiRequestError) {
        if (err.status === 404) {
          setNotFound(true);
        } else {
          setLoadError(err.message);
        }
      } else {
        setLoadError('Failed to load ticket.');
      }
    } finally {
      setLoading(false);
    }
  }, [ticketId]);

  useEffect(() => {
    let cancelled = false;

    async function loadUsers() {
      try {
        const data = await getUsers();
        if (cancelled) {
          return;
        }
        setUsers(data);
        const stored = getStoredCreatedBy();
        const defaultCreatedBy =
          stored && data.some((u) => u.id === stored)
            ? String(stored)
            : data[0]
              ? String(data[0].id)
              : '';
        setCommentCreatedBy(defaultCreatedBy);
      } catch {
        // Assignee/comment dropdowns degrade gracefully; ticket load shows primary errors.
      }
    }

    void loadUsers();
    return () => {
      cancelled = true;
    };
  }, []);

  useEffect(() => {
    if (ticketId === null) {
      setLoading(false);
      return;
    }
    void loadTicket();
  }, [ticketId, loadTicket, reloadKey]);

  const isDirty =
    ticket !== null &&
    (description !== (ticket.description ?? '') ||
      priority !== ticket.priority ||
      assignedTo !== (ticket.assignedTo ? String(ticket.assignedTo) : UNASSIGNED));

  async function handleSave(e: FormEvent) {
    e.preventDefault();
    if (!ticket || ticketId === null || !isDirty) {
      return;
    }

    setSaveError(null);
    setSaving(true);

    try {
      const updated = await updateTicket(ticketId, {
        title: ticket.title,
        description: description.trim() || null,
        priority,
        assignedTo: assignedTo ? Number(assignedTo) : null,
      });
      setTicket(updated);
      const form = syncFormFromTicket(updated);
      setTitle(form.title);
      setDescription(form.description);
      setPriority(form.priority);
      setAssignedTo(form.assignedTo);
    } catch (err) {
      if (err instanceof ApiRequestError) {
        setSaveError(err.message);
      } else {
        setSaveError('An unexpected error occurred while saving.');
      }
    } finally {
      setSaving(false);
    }
  }

  async function handleStatusChange(newStatus: TicketStatus) {
    if (!ticket || ticketId === null) {
      return;
    }

    setStatusSelection(newStatus);
    setStatusError(null);
    setPatchingStatus(true);

    try {
      const updated = await changeTicketStatus(ticketId, { status: newStatus });
      setTicket(updated);
      setStatusSelection(STATUS_PLACEHOLDER);
    } catch (err) {
      setStatusSelection(STATUS_PLACEHOLDER);
      if (err instanceof ApiRequestError) {
        setStatusError(err.message);
      } else {
        setStatusError('An unexpected error occurred while changing status.');
      }
    } finally {
      setPatchingStatus(false);
    }
  }

  async function handleAddComment(e: FormEvent) {
    e.preventDefault();
    if (!ticket || ticketId === null || !commentCreatedBy) {
      return;
    }

    setCommentError(null);
    setSubmittingComment(true);

    try {
      const createdById = Number(commentCreatedBy);
      const newComment = await addComment(ticketId, {
        message: commentMessage.trim(),
        createdBy: createdById,
      });
      setStoredCreatedBy(createdById);
      setTicket({
        ...ticket,
        comments: [...ticket.comments, newComment],
      });
      setCommentMessage('');
    } catch (err) {
      if (err instanceof ApiRequestError) {
        setCommentError(err.message);
      } else {
        setCommentError('An unexpected error occurred while adding the comment.');
      }
    } finally {
      setSubmittingComment(false);
    }
  }

  if (ticketId === null) {
    return (
      <Container maxWidth="sm">
        <Button
          component={RouterLink}
          to="/tickets"
          startIcon={<ArrowBackIcon />}
          sx={{ mb: 3, color: 'text.secondary' }}
        >
          Back to tickets
        </Button>
        <Paper
          elevation={0}
          sx={{
            p: { xs: 4, md: 6 },
            textAlign: 'center',
            border: '1px solid',
            borderColor: 'divider',
            borderRadius: 3,
          }}
        >
          <Typography variant="h5" gutterBottom>
            Invalid ticket ID
          </Typography>
          <Typography color="text.secondary" sx={{ mb: 3 }}>
            The ticket ID in the URL is not valid.
          </Typography>
          <Button component={RouterLink} to="/tickets" variant="contained">
            Return to ticket list
          </Button>
        </Paper>
      </Container>
    );
  }

  if (loading) {
    return (
      <Container maxWidth="lg">
        <LoadingState />
      </Container>
    );
  }

  if (notFound) {
    return (
      <Container maxWidth="sm">
        <Button
          component={RouterLink}
          to="/tickets"
          startIcon={<ArrowBackIcon />}
          sx={{ mb: 3, color: 'text.secondary' }}
        >
          Back to tickets
        </Button>
        <Paper
          elevation={0}
          sx={{
            p: { xs: 4, md: 6 },
            textAlign: 'center',
            border: '1px solid',
            borderColor: 'divider',
            borderRadius: 3,
          }}
        >
          <Box
            sx={{
              width: 80,
              height: 80,
              mx: 'auto',
              mb: 3,
              borderRadius: '50%',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              bgcolor: 'action.hover',
              color: 'text.secondary',
            }}
          >
            <SearchOffIcon sx={{ fontSize: 40 }} />
          </Box>
          <Typography variant="h5" gutterBottom>
            Ticket not found
          </Typography>
          <Typography color="text.secondary" sx={{ mb: 3 }}>
            Ticket #{ticketId} does not exist or may have been removed.
          </Typography>
          <Button component={RouterLink} to="/tickets" variant="contained">
            Return to ticket list
          </Button>
        </Paper>
      </Container>
    );
  }

  if (!ticket) {
    return (
      <Container maxWidth="lg">
        <Button
          component={RouterLink}
          to="/tickets"
          startIcon={<ArrowBackIcon />}
          sx={{ mb: 2, color: 'text.secondary' }}
        >
          Back to tickets
        </Button>
        {loadError && <ErrorBanner message={loadError} />}
        <Box sx={{ mt: 2 }}>
          <Button variant="outlined" onClick={() => setReloadKey((k) => k + 1)}>
            Retry
          </Button>
        </Box>
      </Container>
    );
  }

  const hasStatusOptions = ticket.validNextStatuses.length > 0;

  return (
    <Container maxWidth="lg">
      <Button
        component={RouterLink}
        to="/tickets"
        startIcon={<ArrowBackIcon />}
        sx={{ mb: 2, color: 'text.secondary' }}
      >
        Back to tickets
      </Button>

      <PageHeader
        title={ticket.title}
        subtitle={`Ticket #${ticket.id}`}
      />

      {loadError && <ErrorBanner message={loadError} />}

      <Paper
        elevation={0}
        sx={{
          p: { xs: 2.5, md: 3 },
          mb: 3,
          border: '1px solid',
          borderColor: 'divider',
          borderRadius: 3,
        }}
      >
        <FormSection icon={<ConfirmationNumberOutlinedIcon />} title="Overview">
          <Box
            sx={{
              display: 'flex',
              flexWrap: 'wrap',
              gap: 3,
              alignItems: 'center',
            }}
          >
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <Typography variant="body2" color="text.secondary">
                Status
              </Typography>
              <TicketStatusChip status={ticket.status} />
            </Box>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <Typography variant="body2" color="text.secondary">
                Priority
              </Typography>
              <PriorityChip priority={priority} />
            </Box>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <UserAvatar name={ticket.createdByName} size={28} />
              <Box>
                <Typography variant="body2" color="text.secondary">
                  Created by
                </Typography>
                <Typography variant="body2">{ticket.createdByName}</Typography>
              </Box>
            </Box>
            <Box>
              <Typography variant="body2" color="text.secondary">
                Assignee
              </Typography>
              <Typography variant="body2">
                {ticket.assignedToName ?? 'Unassigned'}
              </Typography>
            </Box>
            <Box>
              <Typography variant="body2" color="text.secondary">
                Created
              </Typography>
              <Typography variant="body2">{formatDate(ticket.createdAt)}</Typography>
            </Box>
            <Box>
              <Typography variant="body2" color="text.secondary">
                Updated
              </Typography>
              <Typography variant="body2">{formatDate(ticket.updatedAt)}</Typography>
            </Box>
          </Box>
        </FormSection>
      </Paper>

      <Paper
        component="form"
        onSubmit={handleSave}
        elevation={0}
        sx={{
          p: { xs: 2.5, md: 4 },
          mb: 3,
          border: '1px solid',
          borderColor: 'divider',
          borderRadius: 3,
        }}
      >
        {saveError && (
          <Box sx={{ mb: 2 }}>
            <ErrorBanner message={saveError} />
          </Box>
        )}

        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3.5 }}>
          <FormSection icon={<DescriptionOutlinedIcon />} title="Ticket details">
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              <TextField
                label="Title"
                required
                fullWidth
                value={title}
                disabled
                slotProps={{
                  input: {
                    readOnly: true,
                  },
                }}
              />
              <TextField
                label="Description"
                fullWidth
                multiline
                minRows={4}
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                disabled={saving}
              />
            </Box>
          </FormSection>

          <Divider />

          <FormSection icon={<FlagOutlinedIcon />} title="Priority">
            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1.5 }}>
              {PRIORITY_OPTIONS.map((p) => {
                const isSelected = priority === p;
                const chipStyle = {
                  Low: { border: '#047857', bg: '#ecfdf5' },
                  Medium: { border: '#b45309', bg: '#fffbeb' },
                  High: { border: '#b91c1c', bg: '#fef2f2' },
                }[p];

                return (
                  <ToggleButton
                    key={p}
                    value={p}
                    selected={isSelected}
                    onChange={() => setPriority(p)}
                    disabled={saving}
                    aria-label={`${p} priority`}
                    aria-pressed={isSelected}
                    sx={{
                      px: 2,
                      py: 0.75,
                      borderRadius: '10px !important',
                      border: '2px solid !important',
                      borderColor: isSelected
                        ? `${chipStyle.border} !important`
                        : 'divider !important',
                      bgcolor: isSelected ? `${chipStyle.bg} !important` : '#fff',
                      boxShadow: isSelected ? `0 0 0 1px ${chipStyle.border}` : 'none',
                      '&.Mui-selected': {
                        bgcolor: `${chipStyle.bg} !important`,
                        borderColor: `${chipStyle.border} !important`,
                      },
                      '&:hover': {
                        bgcolor: isSelected ? chipStyle.bg : 'action.hover',
                      },
                    }}
                  >
                    <PriorityChip priority={p} variant={isSelected ? 'filled' : 'outline'} />
                  </ToggleButton>
                );
              })}
            </Box>
          </FormSection>

          <Divider />

          <FormSection icon={<AssignmentIndOutlinedIcon />} title="Assignment">
            <FormControl fullWidth disabled={saving}>
              <InputLabel id="detail-assignee-label">Assignee</InputLabel>
              <Select
                labelId="detail-assignee-label"
                label="Assignee"
                value={assignedTo}
                onChange={(e) => setAssignedTo(e.target.value)}
              >
                <MenuItem value={UNASSIGNED}>
                  <ListItemText primary="Unassigned" secondary="No agent assigned" />
                </MenuItem>
                {users.map((user) => (
                  <MenuItem key={user.id} value={String(user.id)}>
                    <ListItemIcon sx={{ minWidth: 40 }}>
                      <UserAvatar name={user.name} size={28} />
                    </ListItemIcon>
                    <ListItemText primary={user.name} secondary={user.role} />
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </FormSection>

          <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
            <Button
              type="submit"
              variant="contained"
              disabled={!isDirty || saving}
              sx={{ minWidth: 160 }}
            >
              {saving ? 'Saving…' : 'Save changes'}
            </Button>
          </Box>
        </Box>
      </Paper>

      <Paper
        elevation={0}
        sx={{
          p: { xs: 2.5, md: 4 },
          mb: 3,
          border: '1px solid',
          borderColor: 'divider',
          borderRadius: 3,
        }}
      >
        <FormSection icon={<PendingActionsOutlinedIcon />} title="Status">
          <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2, alignItems: 'flex-start' }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, pt: 1 }}>
              <Typography variant="body2" color="text.secondary">
                Current
              </Typography>
              <TicketStatusChip status={ticket.status} />
            </Box>

            <FormControl
              sx={{ minWidth: 220 }}
              disabled={!hasStatusOptions || patchingStatus}
            >
              <InputLabel id="status-change-label" shrink>
                New status
              </InputLabel>
              <Select
                labelId="status-change-label"
                label="New status"
                value={statusSelection}
                displayEmpty
                renderValue={(selected) =>
                  selected ? (
                    formatStatusLabel(selected as TicketStatus)
                  ) : (
                    <Typography
                      component="span"
                      color="text.secondary"
                      sx={{ fontSize: 'inherit' }}
                    >
                      Select next status…
                    </Typography>
                  )
                }
                onChange={(e) => {
                  const value = e.target.value;
                  if (value !== STATUS_PLACEHOLDER) {
                    void handleStatusChange(value as TicketStatus);
                  }
                }}
              >
                {ticket.validNextStatuses.map((status) => (
                  <MenuItem key={status} value={status}>
                    {formatStatusLabel(status)}
                  </MenuItem>
                ))}
              </Select>
              {!hasStatusOptions && (
                <FormHelperText>No further status changes</FormHelperText>
              )}
            </FormControl>
          </Box>

          {statusError && (
            <Alert severity="warning" sx={{ mt: 2 }}>
              {statusError}
            </Alert>
          )}
        </FormSection>
      </Paper>

      <Paper
        elevation={0}
        sx={{
          p: { xs: 2.5, md: 4 },
          border: '1px solid',
          borderColor: 'divider',
          borderRadius: 3,
        }}
      >
        <FormSection icon={<InboxOutlinedIcon />} title="Comments">
          {ticket.comments.length === 0 ? (
            <Typography color="text.secondary" sx={{ mb: 3 }}>
              No comments yet.
            </Typography>
          ) : (
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mb: 3 }}>
              {ticket.comments.map((comment) => (
                <Box
                  key={comment.id}
                  sx={{
                    p: 2,
                    borderRadius: 2,
                    bgcolor: 'action.hover',
                  }}
                >
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, mb: 1 }}>
                    <UserAvatar name={comment.createdByName} size={32} />
                    <Box sx={{ flex: 1 }}>
                      <Typography variant="subtitle2">{comment.createdByName}</Typography>
                      <Typography variant="caption" color="text.secondary">
                        {formatDate(comment.createdAt)}
                      </Typography>
                    </Box>
                  </Box>
                  <Typography variant="body2" sx={{ whiteSpace: 'pre-wrap' }}>
                    {comment.message}
                  </Typography>
                </Box>
              ))}
            </Box>
          )}

          <Divider sx={{ mb: 3 }} />

          <Box component="form" onSubmit={handleAddComment}>
            {commentError && (
              <Box sx={{ mb: 2 }}>
                <ErrorBanner message={commentError} />
              </Box>
            )}

            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              <TextField
                label="Add a comment"
                fullWidth
                multiline
                minRows={3}
                placeholder="Write your comment…"
                value={commentMessage}
                onChange={(e) => setCommentMessage(e.target.value)}
                disabled={submittingComment}
              />

              <FormSection icon={<PersonOutlinedIcon />} title="Posted by">
                <FormControl fullWidth required disabled={submittingComment}>
                  <InputLabel id="comment-created-by-label">Created By</InputLabel>
                  <Select
                    labelId="comment-created-by-label"
                    label="Created By"
                    value={commentCreatedBy}
                    onChange={(e) => setCommentCreatedBy(e.target.value)}
                  >
                    {users.map((user) => (
                      <MenuItem key={user.id} value={String(user.id)}>
                        <ListItemIcon sx={{ minWidth: 40 }}>
                          <UserAvatar name={user.name} size={28} />
                        </ListItemIcon>
                        <ListItemText primary={user.name} secondary={user.email} />
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </FormSection>

              <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                <Button
                  type="submit"
                  variant="contained"
                  disabled={submittingComment || !commentMessage.trim() || !commentCreatedBy}
                >
                  {submittingComment ? 'Posting…' : 'Post comment'}
                </Button>
              </Box>
            </Box>
          </Box>
        </FormSection>
      </Paper>
    </Container>
  );
}
