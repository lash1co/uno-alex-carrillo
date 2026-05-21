import { axiosClient } from "./api/axiosClient";
import type { Assignee } from "../types/assignee";

export const assigneeService = {
  getAll: async () => {
    const response = await axiosClient.get<Assignee[]>("/assignees");

    return response.data;
  },
};
