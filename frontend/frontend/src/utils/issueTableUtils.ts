import {
  IssuePriority,
  IssueStatus,
  type Issue,
} from "../types/issue";
import type {
  SortDirection,
  SortKey,
} from "../components/issues/IssueTable";

export const priorityLabels: Record<IssuePriority, string> = {
  [IssuePriority.LOW]: "Low",
  [IssuePriority.MEDIUM]: "Medium",
  [IssuePriority.HIGH]: "High",
};

export const statusLabels: Record<IssueStatus, string> = {
  [IssueStatus.OPEN]: "Open",
  [IssueStatus.IN_PROGRESS]: "In Progress",
  [IssueStatus.CLOSED]: "Closed",
};

export const getNextStatus = (
  status: IssueStatus
) => {
  if (status === IssueStatus.OPEN) {
    return IssueStatus.IN_PROGRESS;
  }

  if (status === IssueStatus.IN_PROGRESS) {
    return IssueStatus.CLOSED;
  }

  return IssueStatus.OPEN;
};

export const getNextPriority = (
  priority: IssuePriority
) => {
  if (priority === IssuePriority.LOW) {
    return IssuePriority.MEDIUM;
  }

  if (priority === IssuePriority.MEDIUM) {
    return IssuePriority.HIGH;
  }

  return IssuePriority.LOW;
};

export const filterAndSortIssues = (
  issues: Issue[],
  searchTerm: string,
  sortKey: SortKey,
  sortDirection: SortDirection
) => {
  const normalizedSearch =
    searchTerm.trim().toLowerCase();

  return [...issues]
    .filter((issue) => {
      if (!normalizedSearch) return true;

      const searchableText = [
        issue.title,
        priorityLabels[issue.priority],
        statusLabels[issue.status],
        issue.assignee?.name || "Unassigned",
      ]
        .join(" ")
        .toLowerCase();

      return searchableText.includes(
        normalizedSearch
      );
    })
    .sort((firstIssue, secondIssue) => {
      const direction =
        sortDirection === "asc" ? 1 : -1;

      const getSortValue = (issue: Issue) => {
        if (sortKey === "priority") {
          return priorityLabels[issue.priority];
        }

        if (sortKey === "status") {
          return statusLabels[issue.status];
        }

        if (sortKey === "assignee") {
          return issue.assignee?.name || "";
        }

        return issue.title;
      };

      return getSortValue(firstIssue)
        .localeCompare(
          getSortValue(secondIssue),
          undefined,
          { sensitivity: "base" }
        ) * direction;
    });
};
