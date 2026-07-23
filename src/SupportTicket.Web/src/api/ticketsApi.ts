import type {
  CreateTicketRequest,
  TicketListItem,
  TicketStatus,
} from '../types/api';
import { request } from './client';

export function getTickets(
  search?: string,
  status?: TicketStatus,
): Promise<TicketListItem[]> {
  const params = new URLSearchParams();
  if (search?.trim()) {
    params.set('search', search.trim());
  }
  if (status) {
    params.set('status', status);
  }
  const query = params.toString();
  return request<TicketListItem[]>(`/tickets${query ? `?${query}` : ''}`);
}

export function createTicket(
  body: CreateTicketRequest,
): Promise<TicketListItem> {
  return request<TicketListItem>('/tickets', {
    method: 'POST',
    body: JSON.stringify(body),
  });
}
