import type {
  ChangeStatusRequest,
  Comment,
  CreateCommentRequest,
  CreateTicketRequest,
  TicketDetail,
  TicketListItem,
  TicketStatus,
  UpdateTicketRequest,
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

export function getTicket(id: number): Promise<TicketDetail> {
  return request<TicketDetail>(`/tickets/${id}`);
}

export function updateTicket(
  id: number,
  body: UpdateTicketRequest,
): Promise<TicketDetail> {
  return request<TicketDetail>(`/tickets/${id}`, {
    method: 'PUT',
    body: JSON.stringify(body),
  });
}

export function changeTicketStatus(
  id: number,
  body: ChangeStatusRequest,
): Promise<TicketDetail> {
  return request<TicketDetail>(`/tickets/${id}/status`, {
    method: 'PATCH',
    body: JSON.stringify(body),
  });
}

export function addComment(
  ticketId: number,
  body: CreateCommentRequest,
): Promise<Comment> {
  return request<Comment>(`/tickets/${ticketId}/comments`, {
    method: 'POST',
    body: JSON.stringify(body),
  });
}
