import type { LoginRequest, LoginResponse } from "../types/auth";
import { axiosClient } from "./api/axiosClient";

export const authService = {
  login: async (
    data: LoginRequest
  ) => {
    const response =
      await axiosClient.post<LoginResponse>(
        "/auth/login",
        data
      );

    return response.data;
  },
};