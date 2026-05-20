import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import type { Issue } from "../types/issue";
import { issueService } from "../services/issueService";
import { attachmentService } from "../services/attachmentService";
import { IssueForm } from "../components/issues/IssueForm";
import { FileUpload } from "../components/attachments/FileUpload";

export const IssueDetailPage = () => {
  const { id } = useParams();

  const [issue, setIssue] = useState<Issue | null>(null);

  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!id) return;

    loadIssue(id);
  }, [id]);

  const loadIssue = async (
    issueId: string
  ) => {
    try {
      const data =
        await issueService.getById(issueId);

      setIssue(data);
    } finally {
      setLoading(false);
    }
  };

  const handleUpdate = async (
    values: Partial<Issue>
  ) => {
    if (!id) return;

    await issueService.update(id, values);

    await loadIssue(id);
  };

  const handleUpload = async (
    file: File
  ) => {
    if (!id) return;

    await attachmentService.upload(
      id,
      file
    );

    await loadIssue(id);
  };

  const handleRemoveAttachment = (
    attachmentId: string
  ) => {
    if (!issue) return;

    setIssue({
      ...issue,
      attachments:
        issue.attachments.filter(
          (attachment) =>
            attachment.id !== attachmentId
        ),
    });
  };

  if (loading) {
    return (
      <div className="container">
        <p>Loading issue...</p>
      </div>
    );
  }

  if (!issue) {
    return (
      <div className="container">
        <p>Issue not found</p>
      </div>
    );
  }

  return (
    <div className="container">
      <div className="page-card">
        <div className="page-header">
          <div>
            <h1>Issue Detail</h1>
            <p className="page-subtitle">
              Update the issue and manage its attachments.
            </p>
          </div>

          <Link
            className="button button--secondary"
            to="/issues"
          >
            Back to issues
          </Link>
        </div>

        <IssueForm
          issue={issue}
          onSubmit={handleUpdate}
        />

        <hr className="divider" />

        <div className="section-header">
          <div>
            <h2>Attachments</h2>
            <p>
              Upload images related to this issue.
            </p>
          </div>
        </div>

        <FileUpload
          onUpload={handleUpload}
        />

        {issue.attachments.length === 0 && (
          <p className="empty-state">
            No attachments yet.
          </p>
        )}

        <div className="attachment-grid">
          {issue.attachments.map(
            (attachment) => {
              const isImage =
                attachment.fileUrl.match(
                  /\.(jpg|jpeg|png|gif|webp)$/i
                );

              return (
                <div
                  className="attachment-card"
                  key={attachment.id}
                >
                  {isImage ? (
                    <img
                      src={attachment.fileUrl}
                      alt={
                        attachment.fileName
                      }
                    />
                  ) : (
                    <p>
                      {
                        attachment.fileName
                      }
                    </p>
                  )}

                  <button
                    onClick={() =>
                      handleRemoveAttachment(
                        attachment.id
                      )
                    }
                  >
                    Remove
                  </button>
                </div>
              );
            }
          )}
        </div>
      </div>
    </div>
  );
};
