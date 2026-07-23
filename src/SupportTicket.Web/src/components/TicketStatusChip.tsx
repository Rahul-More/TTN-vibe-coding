import Chip from '@mui/material/Chip';
import type { TicketStatus } from '../types/api';
import { formatStatusLabel } from '../utils/labels';

const statusStyles: Record<
  TicketStatus,
  { bgcolor: string; color: string; borderColor: string }
> = {
  Open: {
    bgcolor: '#eff6ff',
    color: '#1d4ed8',
    borderColor: '#bfdbfe',
  },
  InProgress: {
    bgcolor: '#f5f3ff',
    color: '#6d28d9',
    borderColor: '#ddd6fe',
  },
  Resolved: {
    bgcolor: '#ecfdf5',
    color: '#047857',
    borderColor: '#a7f3d0',
  },
  Closed: {
    bgcolor: '#f8fafc',
    color: '#475569',
    borderColor: '#e2e8f0',
  },
  Cancelled: {
    bgcolor: '#fef2f2',
    color: '#b91c1c',
    borderColor: '#fecaca',
  },
};

interface TicketStatusChipProps {
  status: TicketStatus;
}

export function TicketStatusChip({ status }: TicketStatusChipProps) {
  const style = statusStyles[status];
  return (
    <Chip
      label={formatStatusLabel(status)}
      size="small"
      sx={{
        bgcolor: style.bgcolor,
        color: style.color,
        border: '1px solid',
        borderColor: style.borderColor,
        fontWeight: 600,
      }}
    />
  );
}
