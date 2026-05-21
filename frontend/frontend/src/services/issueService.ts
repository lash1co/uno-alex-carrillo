import { axiosClient } from "./api/axiosClient";
import type {
  CreateIssueRequest,
  Issue,
  UpdateIssueRequest,
} from "../types/issue";
import type { PaginatedResponse } from "../types/pagination";

export const issueService = {
  getAll: async (page: number, pageSize: number) => {
    const response = await axiosClient.get<PaginatedResponse<Issue>>(`/issues?page=${page}&pageSize=${pageSize}`);
    return response.data;
  },

  getById: async (id: string) => {
    const response = await axiosClient.get<Issue>(`/issues/${id}`);
    return response.data;
  },

  create: async (data: CreateIssueRequest) => {
    const response = await axiosClient.post<Issue>(
      "/issues",
      data
    );

    return response.data;
  },

  update: async (id: string, data: UpdateIssueRequest) => {
    const response = await axiosClient.put<Issue>(
      `/issues/${id}`,
      data
    );

    return response.data;
  },

  delete: async (id: string) => {
    await axiosClient.delete(`/issues/${id}`);
  },
};
