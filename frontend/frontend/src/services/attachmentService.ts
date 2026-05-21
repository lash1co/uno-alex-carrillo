import { axiosClient } from "./api/axiosClient";

export const attachmentService = {
  upload: async (
    issueId: string,
    file: File,
    onProgress?: (progress: number) => void
  ) => {
    const formData = new FormData();

    formData.append("file", file);

    const response = await axiosClient.post(
      `/issues/${issueId}/attachments`,
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data",
        },
        onUploadProgress: (event) => {
          if (!event.total) return;

          const progress = Math.round(
            (event.loaded * 100) / event.total
          );

          onProgress?.(progress);
        },
      }
    );

    return response.data;
  },

  delete: async (
    issueId: string,
    attachmentId: string
  ) => {
    await axiosClient.delete(
      `/issues/${issueId}/attachments/${attachmentId}`
    );
  },
};
