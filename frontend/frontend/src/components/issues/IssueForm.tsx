import { useForm } from "react-hook-form";
import type { Assignee } from "../../types/assignee";
import {
  IssuePriority,
  IssueStatus,
  type Issue,
  type UpdateIssueRequest,
} from "../../types/issue";

type Props = {
  issue: Issue;
  assignees: Assignee[];
  assigneesLoading: boolean;
  onSubmit: (data: UpdateIssueRequest) => Promise<void>;
};

export const IssueForm = ({
  issue,
  assignees,
  assigneesLoading,
  onSubmit,
}: Props) => {
  const {
    register,
    handleSubmit,
    formState: { isSubmitting },
  } = useForm<UpdateIssueRequest>({
    defaultValues: {
      status: issue.status,
      priority: issue.priority,
      assigneeId: issue.assigneeId || "",
    },
  });

  const handleFormSubmit = (
    values: UpdateIssueRequest
  ) => {
    return onSubmit({
      ...values,
      assigneeId:
        values.assigneeId || null,
    });
  };

  return (
    <div className="issue-detail-grid">
      <section className="issue-summary">
        <span className="eyebrow">
          Issue
        </span>
        <h2>{issue.title}</h2>
        <p>
          {issue.description ||
            "No description provided."}
        </p>
      </section>

      <form
        className="form issue-edit-form"
        onSubmit={handleSubmit(handleFormSubmit)}
      >
        <div className="form-field">
          <label>Status</label>
          <select
            {...register("status", {
              valueAsNumber: true,
            })}
          >
            <option value={IssueStatus.OPEN}>
              Open
            </option>
            <option value={IssueStatus.IN_PROGRESS}>
              In Progress
            </option>
            <option value={IssueStatus.CLOSED}>
              Closed
            </option>
          </select>
        </div>

        <div className="form-field">
          <label>Priority</label>
          <select
            {...register("priority", {
              valueAsNumber: true,
            })}
          >
            <option value={IssuePriority.LOW}>
              Low
            </option>
            <option value={IssuePriority.MEDIUM}>
              Medium
            </option>
            <option value={IssuePriority.HIGH}>
              High
            </option>
          </select>
        </div>

        <div className="form-field">
          <label>Assignee</label>
          <select
            disabled={assigneesLoading}
            {...register("assigneeId")}
          >
            <option value="">
              Unassigned
            </option>
            {assignees.map((assignee) => (
              <option
                key={assignee.id}
                value={assignee.id}
              >
                {assignee.name}
              </option>
            ))}
          </select>
        </div>

        <button
          type="submit"
          disabled={isSubmitting}
        >
          Save changes
        </button>
      </form>
    </div>
  );
};
