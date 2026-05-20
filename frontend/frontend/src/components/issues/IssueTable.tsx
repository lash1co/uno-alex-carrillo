import { Link } from "react-router-dom";
import {
  IssuePriority,
  IssueStatus,
  type Issue,
} from "../../types/issue";

type Props = {
  issues: Issue[];
};

const priorityLabels: Record<IssuePriority, string> = {
  [IssuePriority.LOW]: "Low",
  [IssuePriority.MEDIUM]: "Medium",
  [IssuePriority.HIGH]: "High",
};

const statusLabels: Record<IssueStatus, string> = {
  [IssueStatus.OPEN]: "Open",
  [IssueStatus.IN_PROGRESS]: "In Progress",
  [IssueStatus.CLOSED]: "Closed",
};

const priorityClasses: Record<IssuePriority, string> = {
  [IssuePriority.LOW]: "badge--low",
  [IssuePriority.MEDIUM]: "badge--medium",
  [IssuePriority.HIGH]: "badge--high",
};

const statusClasses: Record<IssueStatus, string> = {
  [IssueStatus.OPEN]: "badge--open",
  [IssueStatus.IN_PROGRESS]: "badge--in-progress",
  [IssueStatus.CLOSED]: "badge--closed",
};

export const IssueTable = ({
  issues,
}: Props) => {
  return (
    <div className="table-shell">
      <table className="issue-table">
        <thead>
          <tr>
            <th>Title</th>
            <th>Priority</th>
            <th>Status</th>
            <th>Assignee</th>
          </tr>
        </thead>

        <tbody>
          {issues.length === 0 && (
            <tr>
              <td
                className="issue-table__empty"
                colSpan={4}
              >
                No issues found.
              </td>
            </tr>
          )}

          {issues.map((issue) => (
            <tr key={issue.id}>
              <td
                className="issue-table__title"
                data-label="Title"
              >
                <Link to={`/issues/${issue.id}`}>
                  {issue.title}
                </Link>
              </td>

              <td data-label="Priority">
                <span
                  className={`badge ${priorityClasses[issue.priority]}`}
                >
                  {priorityLabels[issue.priority]}
                </span>
              </td>

              <td data-label="Status">
                <span
                  className={`badge ${statusClasses[issue.status]}`}
                >
                  {statusLabels[issue.status]}
                </span>
              </td>

              <td data-label="Assignee">
                {issue.assignee || "Unassigned"}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
