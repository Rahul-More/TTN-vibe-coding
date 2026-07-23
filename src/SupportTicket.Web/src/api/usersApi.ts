import type { User } from '../types/api';
import { request } from './client';

export function getUsers(): Promise<User[]> {
  return request<User[]>('/users');
}
