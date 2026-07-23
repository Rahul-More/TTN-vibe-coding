import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import AssignmentIndOutlinedIcon from '@mui/icons-material/AssignmentIndOutlined';
import DescriptionOutlinedIcon from '@mui/icons-material/DescriptionOutlined';
import FlagOutlinedIcon from '@mui/icons-material/FlagOutlined';
import PersonOutlinedIcon from '@mui/icons-material/PersonOutlined';
import type { ReactNode } from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Container from '@mui/material/Container';
import Divider from '@mui/material/Divider';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import MenuItem from '@mui/material/MenuItem';
import Paper from '@mui/material/Paper';
import Select from '@mui/material/Select';
import TextField from '@mui/material/TextField';
import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';
import Typography from '@mui/material/Typography';
import { type FormEvent, useEffect, useState } from 'react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { createTicket } from '../api/ticketsApi';
import { getUsers } from '../api/usersApi';
import { ApiRequestError } from '../api/client';
import { ErrorBanner } from '../components/ErrorBanner';
import { LoadingState } from '../components/LoadingState';
import { PageHeader } from '../components/layout/PageHeader';
import { PriorityChip } from '../components/PriorityChip';
import { UserAvatar } from '../components/UserAvatar';
import type { TicketPriority, User } from '../types/api';
import {
  getStoredCreatedBy,
  setStoredCreatedBy,
} from '../utils/createdByStorage';

const PRIORITY_OPTIONS: TicketPriority[] = ['Low', 'Medium', 'High'];

const UNASSIGNED = '';

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

export function CreateTicketPage() {
  const navigate = useNavigate();

  const [users, setUsers] = useState<User[]>([]);
  const [loadingUsers, setLoadingUsers] = useState(true);
  const [loadError, setLoadError] = useState<string | null>(null);

  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [priority, setPriority] = useState<TicketPriority>('Medium');
  const [assignedTo, setAssignedTo] = useState<string>(UNASSIGNED);
  const [createdBy, setCreatedBy] = useState<string>('');

  const [submitting, setSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    async function loadUsers() {
      setLoadingUsers(true);
      setLoadError(null);
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
        setCreatedBy(defaultCreatedBy);
      } catch (err) {
        if (!cancelled) {
          if (err instanceof ApiRequestError) {
            setLoadError(err.message);
          } else {
            setLoadError('Failed to load users.');
          }
        }
      } finally {
        if (!cancelled) {
          setLoadingUsers(false);
        }
      }
    }

    void loadUsers();
    return () => {
      cancelled = true;
    };
  }, []);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setSubmitError(null);
    setSubmitting(true);

    try {
      const createdById = Number(createdBy);
      await createTicket({
        title: title.trim(),
        description: description.trim() || undefined,
        priority,
        assignedTo: assignedTo ? Number(assignedTo) : null,
        createdBy: createdById,
      });
      setStoredCreatedBy(createdById);
      navigate('/tickets');
    } catch (err) {
      if (err instanceof ApiRequestError) {
        setSubmitError(err.message);
      } else {
        setSubmitError('An unexpected error occurred while creating the ticket.');
      }
    } finally {
      setSubmitting(false);
    }
  }

  if (loadingUsers) {
    return (
      <Container maxWidth="md">
        <LoadingState />
      </Container>
    );
  }

  return (
    <Container maxWidth="md">
      <Button
        component={RouterLink}
        to="/tickets"
        startIcon={<ArrowBackIcon />}
        sx={{ mb: 2, color: 'text.secondary' }}
      >
        Back to tickets
      </Button>

      <PageHeader
        title="Create Ticket"
        subtitle="Submit a new support request. All new tickets start with Open status."
      />

      {loadError && <ErrorBanner message={loadError} />}
      {submitError && <ErrorBanner message={submitError} />}

      <Paper
        component="form"
        onSubmit={handleSubmit}
        elevation={0}
        sx={{
          p: { xs: 2.5, md: 4 },
          border: '1px solid',
          borderColor: 'divider',
          borderRadius: 3,
        }}
      >
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3.5 }}>
          <FormSection icon={<DescriptionOutlinedIcon />} title="Ticket details">
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              <TextField
                label="Title"
                required
                fullWidth
                placeholder="Brief summary of the issue"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                disabled={submitting || !!loadError}
              />

              <TextField
                label="Description"
                fullWidth
                multiline
                minRows={4}
                placeholder="Provide additional context, steps to reproduce, or expected behavior…"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                disabled={submitting || !!loadError}
              />
            </Box>
          </FormSection>

          <Divider />

          <FormSection icon={<FlagOutlinedIcon />} title="Priority">
            <Box>
              <ToggleButtonGroup
                exclusive
                value={priority}
                onChange={(_, value: TicketPriority | null) => {
                  if (value) {
                    setPriority(value);
                  }
                }}
                disabled={submitting || !!loadError}
                sx={{ flexWrap: 'wrap', gap: 1 }}
              >
                {PRIORITY_OPTIONS.map((p) => (
                  <ToggleButton
                    key={p}
                    value={p}
                    sx={{
                      px: 2.5,
                      py: 1,
                      borderRadius: '10px !important',
                      border: '1px solid !important',
                      borderColor: 'divider !important',
                      '&.Mui-selected': {
                        bgcolor: 'transparent',
                        borderColor: 'primary.main !important',
                      },
                    }}
                  >
                    <PriorityChip priority={p} />
                  </ToggleButton>
                ))}
              </ToggleButtonGroup>
            </Box>
          </FormSection>

          <Divider />

          <FormSection icon={<AssignmentIndOutlinedIcon />} title="Assignment">
            <FormControl fullWidth disabled={submitting || !!loadError}>
              <InputLabel id="assignee-label">Assignee</InputLabel>
              <Select
                labelId="assignee-label"
                label="Assignee"
                value={assignedTo}
                onChange={(e) => setAssignedTo(e.target.value)}
              >
                <MenuItem value={UNASSIGNED}>
                  <ListItemText primary="Unassigned" secondary="No agent assigned yet" />
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

          <Divider />

          <FormSection icon={<PersonOutlinedIcon />} title="Reporter">
            <FormControl fullWidth required disabled={submitting || !!loadError}>
              <InputLabel id="created-by-label">Created By</InputLabel>
              <Select
                labelId="created-by-label"
                label="Created By"
                value={createdBy}
                onChange={(e) => setCreatedBy(e.target.value)}
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

          <Box
            sx={{
              display: 'flex',
              gap: 2,
              justifyContent: 'flex-end',
              pt: 1,
              borderTop: '1px solid',
              borderColor: 'divider',
            }}
          >
            <Button
              component={RouterLink}
              to="/tickets"
              size="large"
              disabled={submitting}
            >
              Cancel
            </Button>
            <Button
              type="submit"
              variant="contained"
              size="large"
              disabled={submitting || !!loadError || !createdBy}
              sx={{ minWidth: 160 }}
            >
              {submitting ? 'Creating…' : 'Create Ticket'}
            </Button>
          </Box>
        </Box>
      </Paper>
    </Container>
  );
}
