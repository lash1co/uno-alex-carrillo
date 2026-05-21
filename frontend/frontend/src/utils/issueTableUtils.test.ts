import {
  describe,
  expect,
  it,
} from "vitest";
import {
  IssuePriority,
  IssueStatus,
  type Issue,
} from "../types/issue";
import {
  filterAndSortIssues,
  getNextPriority,
} from "./issueTableUtils";

const makeIssue = (
  overrides: Partial<Issue>
): Issue => ({
  id: crypto.randomUUID(),
  title: "Issue",
  description: "Description",
  priority: IssuePriority.LOW,
  status: IssueStatus.OPEN,
  attachments: [],
  ...overrides,
});

describe("issueTableUtils", () => {
  it("filters issues by visible column values", () => {
    const issues = [
      makeIssue({
        title: "Payment bug",
        assignee: {
          id: "1",
          name: "Ana",
        },
      }),
      makeIssue({
        title: "Layout task",
        assignee: {
          id: "2",
          name: "Luis",
        },
      }),
    ];

    const result = filterAndSortIssues(
      issues,
      "ana",
      "title",
      "asc"
    );

    expect(result).toHaveLength(1);
    expect(result[0].title).toBe("Payment bug");
  });

  it("sorts issues by title direction", () => {
    const issues = [
      makeIssue({ title: "Beta" }),
      makeIssue({ title: "Alpha" }),
    ];

    const result = filterAndSortIssues(
      issues,
      "",
      "title",
      "asc"
    );

    expect(result.map((issue) => issue.title))
      .toEqual(["Alpha", "Beta"]);
  });

  it("cycles priority for optimistic toggles", () => {
    expect(getNextPriority(IssuePriority.LOW))
      .toBe(IssuePriority.MEDIUM);
    expect(getNextPriority(IssuePriority.MEDIUM))
      .toBe(IssuePriority.HIGH);
    expect(getNextPriority(IssuePriority.HIGH))
      .toBe(IssuePriority.LOW);
  });
});
