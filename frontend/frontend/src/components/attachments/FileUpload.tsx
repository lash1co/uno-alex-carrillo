import { useState } from "react";

type Props = {
  onUpload: (file: File) => Promise<void>;
};

const MAX_FILE_SIZE =
  5 * 1024 * 1024;

export const FileUpload = ({
  onUpload,
}: Props) => {
  const [progress, setProgress] =
    useState(0);

  const [isUploading, setIsUploading] =
    useState(false);

  const handleChange = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const file =
      event.target.files?.[0];

    if (!file) return;

    // Validate size
    if (
      file.size > MAX_FILE_SIZE
    ) {
      alert(
        "File size cannot exceed 5MB"
      );

      return;
    }

    // Validate image type
    if (
      !file.type.startsWith(
        "image/"
      )
    ) {
      alert(
        "Only image files are allowed"
      );

      return;
    }

    try {
      setIsUploading(true);

      setProgress(10);

      await onUpload(file);

      setProgress(100);
    } finally {
      setIsUploading(false);

      setTimeout(() => {
        setProgress(0);
      }, 1000);
    }
  };

  return (
    <div className="file-upload">
      <input
        type="file"
        accept="image/*"
        disabled={isUploading}
        onChange={handleChange}
      />

      {progress > 0 && (
        <p className="page-subtitle">
          Uploading: {progress}%
        </p>
      )}
    </div>
  );
};
