import type { TicketStatus } from '../types/api';

export function formatStatusLabel(status: TicketStatus): string {
  const labels: Record<TicketStatus, string> = {
    Open: 'Open',
    InProgress: 'In Progress',
    Resolved: 'Resolved',
    Closed: 'Closed',
    Cancelled: 'Cancelled',
  };
  return labels[status];
}

export function getInitials(name: string): string {
  return name
    .split(' ')
    .map((part) => part[0])
    .join('')
    .slice(0, 2)
    .toUpperCase();
}
