export type TicketPriority = 'Low' | 'Medium' | 'High';

export type TicketStatus =
  | 'Open'
  | 'InProgress'
  | 'Resolved'
  | 'Closed'
  | 'Cancelled';

export interface User {
  id: number;
  name: string;
  email: string;
  role: string;
}

export interface TicketListItem {
  id: number;
  title: string;
  description: string | null;
  priority: TicketPriority;
  status: TicketStatus;
  assignedTo: number | null;
  assignedToName: string | null;
  createdBy: number;
  createdByName: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateTicketRequest {
  title: string;
  description?: string;
  priority: TicketPriority;
  assignedTo?: number | null;
  createdBy: number;
}

export interface Comment {
  id: number;
  message: string;
  createdBy: number;
  createdByName: string;
  createdAt: string;
}

export interface TicketDetail extends TicketListItem {
  validNextStatuses: TicketStatus[];
  comments: Comment[];
}

export interface UpdateTicketRequest {
  title: string;
  description?: string | null;
  priority: TicketPriority;
  assignedTo?: number | null;
}

export interface ChangeStatusRequest {
  status: TicketStatus;
}

export interface CreateCommentRequest {
  message: string;
  createdBy: number;
}

export interface ApiErrorBody {
  error: string;
  code?: string;
}
