import AddIcon from '@mui/icons-material/Add';
import FilterListIcon from '@mui/icons-material/FilterList';
import SearchIcon from '@mui/icons-material/Search';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Container from '@mui/material/Container';
import FormControl from '@mui/material/FormControl';
import InputAdornment from '@mui/material/InputAdornment';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import Paper from '@mui/material/Paper';
import Select from '@mui/material/Select';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getTickets } from '../api/ticketsApi';
import { ApiRequestError } from '../api/client';
import { EmptyState } from '../components/EmptyState';
import { ErrorBanner } from '../components/ErrorBanner';
import { LoadingState } from '../components/LoadingState';
import { PageHeader } from '../components/layout/PageHeader';
import { PriorityChip } from '../components/PriorityChip';
import { TicketStatsBar } from '../components/TicketStatsBar';
import { TicketStatusChip } from '../components/TicketStatusChip';
import { UserAvatar } from '../components/UserAvatar';
import { useDebounce } from '../hooks/useDebounce';
import type { TicketListItem, TicketStatus } from '../types/api';
import { formatDate } from '../utils/formatDate';

const STATUS_OPTIONS: { value: TicketStatus | ''; label: string }[] = [
  { value: '', label: 'All statuses' },
  { value: 'Open', label: 'Open' },
  { value: 'InProgress', label: 'In Progress' },
  { value: 'Resolved', label: 'Resolved' },
  { value: 'Closed', label: 'Closed' },
  { value: 'Cancelled', label: 'Cancelled' },
];

export function TicketListPage() {
  const navigate = useNavigate();
  const [search, setSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState<TicketStatus | ''>('');
  const debouncedSearch = useDebounce(search);

  const [tickets, setTickets] = useState<TicketListItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const hasActiveFilters = Boolean(debouncedSearch || statusFilter);

  useEffect(() => {
    let cancelled = false;

    async function loadTickets() {
      setLoading(true);
      setError(null);
      try {
        const data = await getTickets(
          debouncedSearch || undefined,
          statusFilter || undefined,
        );
        if (!cancelled) {
          setTickets(data);
        }
      } catch (err) {
        if (!cancelled) {
          if (err instanceof ApiRequestError) {
            setError(err.message);
          } else {
            setError('An unexpected error occurred while loading tickets.');
          }
          setTickets([]);
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    }

    void loadTickets();
    return () => {
      cancelled = true;
    };
  }, [debouncedSearch, statusFilter]);

  return (
    <Container maxWidth="lg">
      <PageHeader
        title="Support Tickets"
        subtitle="Search, filter, and manage support requests across your team."
        action={
          <Button
            variant="contained"
            size="large"
            startIcon={<AddIcon />}
            onClick={() => navigate('/tickets/new')}
            sx={{ px: 3 }}
          >
            Create Ticket
          </Button>
        }
      />

      {!loading && !error && !hasActiveFilters && tickets.length > 0 && (
        <TicketStatsBar tickets={tickets} />
      )}

      <Paper
        elevation={0}
        sx={{
          p: 2.5,
          mb: 3,
          border: '1px solid',
          borderColor: 'divider',
          borderRadius: 3,
        }}
      >
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
          <FilterListIcon color="primary" fontSize="small" />
          <Typography variant="subtitle1">Filters</Typography>
        </Box>
        <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
          <TextField
            placeholder="Search by title or description…"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            size="small"
            sx={{ minWidth: 280, flexGrow: 1 }}
            slotProps={{
              input: {
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon color="action" fontSize="small" />
                  </InputAdornment>
                ),
              },
            }}
          />
          <FormControl size="small" sx={{ minWidth: 200 }}>
            <InputLabel id="status-filter-label">Status</InputLabel>
            <Select
              labelId="status-filter-label"
              label="Status"
              value={statusFilter}
              onChange={(e) =>
                setStatusFilter(e.target.value as TicketStatus | '')
              }
            >
              {STATUS_OPTIONS.map((option) => (
                <MenuItem key={option.label} value={option.value}>
                  {option.label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          {hasActiveFilters && (
            <Button
              variant="text"
              onClick={() => {
                setSearch('');
                setStatusFilter('');
              }}
            >
              Clear filters
            </Button>
          )}
        </Box>
      </Paper>

      {error && <ErrorBanner message={error} />}

      {loading ? (
        <LoadingState />
      ) : tickets.length === 0 && !error ? (
        <EmptyState
          title="No tickets found"
          description={
            hasActiveFilters
              ? 'Try adjusting your search or status filter to find what you are looking for.'
              : 'Get started by creating your first support ticket.'
          }
          action={
            hasActiveFilters ? undefined : (
              <Button
                variant="contained"
                startIcon={<AddIcon />}
                onClick={() => navigate('/tickets/new')}
              >
                Create Ticket
              </Button>
            )
          }
        />
      ) : (
        <TableContainer
          component={Paper}
          elevation={0}
          sx={{
            border: '1px solid',
            borderColor: 'divider',
            borderRadius: 3,
            overflow: 'hidden',
          }}
        >
          <Box
            sx={{
              px: 2.5,
              py: 2,
              borderBottom: '1px solid',
              borderColor: 'divider',
              bgcolor: '#fafbfc',
            }}
          >
            <Typography variant="subtitle1">
              {tickets.length} ticket{tickets.length === 1 ? '' : 's'}
              {hasActiveFilters ? ' matching filters' : ''}
            </Typography>
          </Box>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Title</TableCell>
                <TableCell>Priority</TableCell>
                <TableCell>Status</TableCell>
                <TableCell>Assignee</TableCell>
                <TableCell>Created</TableCell>
                <TableCell>Updated</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {tickets.map((ticket) => (
                <TableRow
                  key={ticket.id}
                  hover
                  sx={{
                    cursor: 'pointer',
                    '&:last-child td': { borderBottom: 0 },
                    transition: 'background-color 0.15s ease',
                  }}
                  onClick={() => navigate(`/tickets/${ticket.id}`)}
                >
                  <TableCell>
                    <Typography variant="body2" sx={{ fontWeight: 600 }}>
                      {ticket.title}
                    </Typography>
                    {ticket.description && (
                      <Typography
                        variant="caption"
                        color="text.secondary"
                        sx={{
                          display: 'block',
                          maxWidth: 320,
                          overflow: 'hidden',
                          textOverflow: 'ellipsis',
                          whiteSpace: 'nowrap',
                        }}
                      >
                        {ticket.description}
                      </Typography>
                    )}
                  </TableCell>
                  <TableCell>
                    <PriorityChip priority={ticket.priority} />
                  </TableCell>
                  <TableCell>
                    <TicketStatusChip status={ticket.status} />
                  </TableCell>
                  <TableCell>
                    {ticket.assignedToName ? (
                      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <UserAvatar name={ticket.assignedToName} size={28} />
                        <Typography variant="body2">{ticket.assignedToName}</Typography>
                      </Box>
                    ) : (
                      <Typography variant="body2" color="text.secondary">
                        Unassigned
                      </Typography>
                    )}
                  </TableCell>
                  <TableCell>
                    <Typography variant="body2" color="text.secondary">
                      {formatDate(ticket.createdAt)}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Typography variant="body2" color="text.secondary">
                      {formatDate(ticket.updatedAt)}
                    </Typography>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}
    </Container>
  );
}
