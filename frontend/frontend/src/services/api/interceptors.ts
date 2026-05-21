import { toast } from "react-toastify";
import { axiosClient } from "./axiosClient";

axiosClient.interceptors.request.use(
  (config) => {
    const token =
      localStorage.getItem("token");

    if (token) {
      config.headers.Authorization =
        `Bearer ${token}`;
    }

    return config;
  }
);

axiosClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (
      error.response?.status === 401
    ) {
      localStorage.removeItem("token");
      localStorage.removeItem("user");

      window.location.href =
        "/login";
    }

    toast.error(
      error.response?.data?.message ||
      "Something went wrong"
    );

    return Promise.reject(error);
  }
);