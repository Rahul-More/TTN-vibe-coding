import type { ApiErrorBody } from '../types/api';

export class ApiRequestError extends Error {
  readonly code?: string;
  readonly isNetworkError: boolean;

  constructor(message: string, code?: string, isNetworkError = false) {
    super(message);
    this.name = 'ApiRequestError';
    this.code = code;
    this.isNetworkError = isNetworkError;
  }
}

function getBaseUrl(): string {
  const baseUrl = import.meta.env.VITE_API_URL;
  if (!baseUrl) {
    throw new ApiRequestError(
      'VITE_API_URL is not configured. Copy .env.example to .env and set the API URL.',
    );
  }
  return baseUrl.replace(/\/$/, '');
}

export async function request<T>(
  path: string,
  options: RequestInit = {},
): Promise<T> {
  const url = `${getBaseUrl()}${path.startsWith('/') ? path : `/${path}`}`;

  let response: Response;
  try {
    response = await fetch(url, {
      ...options,
      headers: {
        'Content-Type': 'application/json',
        ...options.headers,
      },
    });
  } catch {
    throw new ApiRequestError(
      'Unable to connect to the API. Check that the backend is running.',
      undefined,
      true,
    );
  }

  if (response.status === 204) {
    return undefined as T;
  }

  const body = (await response.json()) as T | ApiErrorBody;

  if (!response.ok) {
    const errorBody = body as ApiErrorBody;
    throw new ApiRequestError(
      errorBody.error ?? 'An unexpected error occurred',
      errorBody.code,
    );
  }

  return body as T;
}
