import CheckCircleOutlinedIcon from '@mui/icons-material/CheckCircleOutlined';
import ConfirmationNumberOutlinedIcon from '@mui/icons-material/ConfirmationNumberOutlined';
import PendingActionsOutlinedIcon from '@mui/icons-material/PendingActionsOutlined';
import PlayCircleOutlinedIcon from '@mui/icons-material/PlayCircleOutlined';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import type { ReactNode } from 'react';
import type { TicketListItem } from '../types/api';

interface StatCardProps {
  label: string;
  value: number;
  icon: ReactNode;
  accent: string;
}

function StatCard({ label, value, icon, accent }: StatCardProps) {
  return (
    <Paper
      elevation={0}
      sx={{
        p: 2.5,
        flex: 1,
        minWidth: 140,
        border: '1px solid',
        borderColor: 'divider',
        borderRadius: 3,
        display: 'flex',
        alignItems: 'center',
        gap: 2,
        transition: 'transform 0.15s ease, box-shadow 0.15s ease',
        '&:hover': {
          transform: 'translateY(-2px)',
          boxShadow: '0 8px 24px rgba(15,23,42,0.08)',
        },
      }}
    >
      <Box
        sx={{
          width: 48,
          height: 48,
          borderRadius: 2.5,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          bgcolor: `${accent}18`,
          color: accent,
        }}
      >
        {icon}
      </Box>
      <Box>
        <Typography variant="h5" sx={{ fontWeight: 700, lineHeight: 1 }}>
          {value}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {label}
        </Typography>
      </Box>
    </Paper>
  );
}

interface TicketStatsBarProps {
  tickets: TicketListItem[];
}

export function TicketStatsBar({ tickets }: TicketStatsBarProps) {
  const open = tickets.filter((t) => t.status === 'Open').length;
  const inProgress = tickets.filter((t) => t.status === 'InProgress').length;
  const resolved = tickets.filter((t) => t.status === 'Resolved').length;

  return (
    <Box
      sx={{
        display: 'flex',
        gap: 2,
        mb: 3,
        flexWrap: 'wrap',
      }}
    >
      <StatCard
        label="Total tickets"
        value={tickets.length}
        icon={<ConfirmationNumberOutlinedIcon />}
        accent="#4f46e5"
      />
      <StatCard
        label="Open"
        value={open}
        icon={<PendingActionsOutlinedIcon />}
        accent="#0ea5e9"
      />
      <StatCard
        label="In progress"
        value={inProgress}
        icon={<PlayCircleOutlinedIcon />}
        accent="#8b5cf6"
      />
      <StatCard
        label="Resolved"
        value={resolved}
        icon={<CheckCircleOutlinedIcon />}
        accent="#10b981"
      />
    </Box>
  );
}
