import {
  ArrowDown,
  ArrowUp,
  Trash2,
} from "lucide-react";
import { Link } from "react-router-dom";
import {
  type Issue,
} from "../../types/issue";
import {
  priorityLabels,
  statusLabels,
} from "../../utils/issueTableUtils";

type Props = {
  issues: Issue[];
  onDelete: (issue: Issue) => void;
  onTogglePriority: (issue: Issue) => void;
  sortKey: SortKey;
  sortDirection: SortDirection;
  onSort: (key: SortKey) => void;
};

export type SortKey =
  | "title"
  | "priority"
  | "status"
  | "assignee";

export type SortDirection =
  | "asc"
  | "desc";

const priorityClasses = {
  0: "badge--low",
  1: "badge--medium",
  2: "badge--high",
};

const statusClasses = {
  0: "badge--open",
  1: "badge--in-progress",
  2: "badge--closed",
};

export const IssueTable = ({
  issues,
  onDelete,
  onTogglePriority,
  sortKey,
  sortDirection,
  onSort,
}: Props) => {
  const getSortIndicator = (key: SortKey) => {
    if (sortKey !== key) return null;

    return (
      <span
        className="sort-indicator"
        aria-hidden="true"
      >
        {sortDirection === "asc" ? (
          <ArrowUp size={14} />
        ) : (
          <ArrowDown size={14} />
        )}
      </span>
    );
  };

  const getAriaSort = (
    key: SortKey
  ): "none" | "ascending" | "descending" => {
    if (sortKey !== key) return "none";

    return sortDirection === "asc"
      ? "ascending"
      : "descending";
  };

  return (
    <div className="table-shell">
      <table className="issue-table">
        <thead>
          <tr>
            <th aria-sort={getAriaSort("title")}>
              <button
                className="table-sort-button"
                type="button"
                onClick={() => onSort("title")}
              >
                Title{getSortIndicator("title")}
              </button>
            </th>
            <th aria-sort={getAriaSort("priority")}>
              <button
                className="table-sort-button"
                type="button"
                onClick={() => onSort("priority")}
              >
                Priority{getSortIndicator("priority")}
              </button>
            </th>
            <th aria-sort={getAriaSort("status")}>
              <button
                className="table-sort-button"
                type="button"
                onClick={() => onSort("status")}
              >
                Status{getSortIndicator("status")}
              </button>
            </th>
            <th aria-sort={getAriaSort("assignee")}>
              <button
                className="table-sort-button"
                type="button"
                onClick={() => onSort("assignee")}
              >
                Assignee{getSortIndicator("assignee")}
              </button>
            </th>
            <th>Actions</th>
          </tr>
        </thead>

        <tbody>
          {issues.length === 0 && (
            <tr>
              <td
                className="issue-table__empty"
                colSpan={5}
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
                <button
                  className={`badge badge-toggle ${priorityClasses[issue.priority]}`}
                  type="button"
                  data-tooltip="Change priority"
                  onClick={() =>
                    onTogglePriority(issue)
                  }
                >
                  {priorityLabels[issue.priority]}
                </button>
              </td>

              <td data-label="Status">
                <span
                  className={`badge ${statusClasses[issue.status]}`}
                >
                  {statusLabels[issue.status]}
                </span>
              </td>

              <td data-label="Assignee">
                {issue.assignee?.name || "Unassigned"}
              </td>

              <td data-label="Actions">
                <button
                  className="icon-button icon-button--danger"
                  type="button"
                  aria-label={`Delete ${issue.title}`}
                  data-tooltip="Delete issue"
                  onClick={() => onDelete(issue)}
                >
                  <Trash2
                    aria-hidden="true"
                    size={18}
                    strokeWidth={2.2}
                  />
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
