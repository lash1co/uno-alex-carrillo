import { useEffect, useState } from "react";
import {
  Link,
  useNavigate,
  useParams,
} from "react-router-dom";
import { Trash2 } from "lucide-react";
import { toast } from "react-toastify";
import type {
  Attachment,
  Issue,
  UpdateIssueRequest,
} from "../types/issue";
import { issueService } from "../services/issueService";
import { attachmentService } from "../services/attachmentService";
import { IssueForm } from "../components/issues/IssueForm";
import { FileUpload } from "../components/attachments/FileUpload";
import { useAssignees } from "../hooks/useAssignees";
import { resolveApiAssetUrl } from "../services/api/url";

const imageExtensions =
  /\.(jpg|jpeg|png|gif|webp|bmp|svg)$/i;

export const IssueDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [issue, setIssue] = useState<Issue | null>(null);

  const [
    attachmentToDelete,
    setAttachmentToDelete,
  ] = useState<Attachment | null>(null);

  const [loading, setLoading] = useState(true);

  const {
    assignees,
    loading: assigneesLoading,
  } = useAssignees();

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
    values: UpdateIssueRequest
  ) => {
    if (!id) return;

    await issueService.update(id, values);

    toast.success("Issue updated successfully");
    navigate("/issues");
  };

  const handleUpload = async (
    file: File
  ) => {
    if (!id) return;

    await attachmentService.upload(
      id,
      file
    );

    toast.success("Attachment uploaded successfully");
    await loadIssue(id);
  };

  const handleRemoveAttachment = async (
    attachment: Attachment
  ) => {
    if (!issue || !id) return;

    await attachmentService.delete(
      id,
      attachment.id
    );

    toast.success("Attachment removed successfully");
    setAttachmentToDelete(null);
    setIssue({
      ...issue,
      attachments:
        issue.attachments.filter(
          (currentAttachment) =>
            currentAttachment.id !== attachment.id
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
          assignees={assignees}
          assigneesLoading={assigneesLoading}
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
                imageExtensions.test(
                  attachment.fileName
                ) ||
                imageExtensions.test(
                  attachment.fileUrl
                );

              const fileUrl =
                resolveApiAssetUrl(
                  attachment.fileUrl
                );

              return (
                <div
                  className="attachment-card"
                  key={attachment.id}
                >
                  {isImage ? (
                    <a
                      className="attachment-preview"
                      href={fileUrl}
                      target="_blank"
                      rel="noreferrer"
                    >
                      <img
                        src={fileUrl}
                        alt={
                          attachment.fileName
                        }
                      />
                    </a>
                  ) : (
                    <p className="attachment-file">
                      Preview unavailable
                    </p>
                  )}

                  <p className="attachment-name">
                    {attachment.fileName}
                  </p>

                  <button
                    className="icon-button icon-button--danger attachment-remove-button"
                    type="button"
                    aria-label={`Delete ${attachment.fileName}`}
                    data-tooltip="Delete attachment"
                    onClick={() =>
                      setAttachmentToDelete(
                        attachment
                      )
                    }
                  >
                    <Trash2
                      aria-hidden="true"
                      size={18}
                      strokeWidth={2.2}
                    />
                  </button>
                </div>
              );
            }
          )}
        </div>

        {attachmentToDelete && (
          <div className="modal-backdrop">
            <div
              className="modal modal--compact"
              role="dialog"
              aria-modal="true"
              aria-labelledby="delete-attachment-title"
            >
              <div className="modal-header">
                <div>
                  <h2 id="delete-attachment-title">
                    Delete attachment
                  </h2>
                  <p>
                    This file will be permanently removed.
                  </p>
                </div>
              </div>

              <p className="modal-copy">
                Are you sure you want to delete{" "}
                <strong>
                  {attachmentToDelete.fileName}
                </strong>
                ?
              </p>

              <div className="form-actions">
                <button
                  className="button button--secondary"
                  type="button"
                  onClick={() =>
                    setAttachmentToDelete(null)
                  }
                >
                  Cancel
                </button>
                <button
                  className="button button--danger"
                  type="button"
                  onClick={() =>
                    handleRemoveAttachment(
                      attachmentToDelete
                    )
                  }
                >
                  Delete attachment
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
