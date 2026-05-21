import {
  render,
  screen,
} from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { MemoryRouter } from "react-router-dom";
import {
  describe,
  expect,
  it,
  vi,
} from "vitest";
import {
  IssuePriority,
  IssueStatus,
  type Issue,
} from "../../types/issue";
import { IssueTable } from "./IssueTable";

const issue: Issue = {
  id: "issue-1",
  title: "Broken login",
  description: "Cannot login",
  priority: IssuePriority.HIGH,
  status: IssueStatus.OPEN,
  assignee: {
    id: "user-1",
    name: "Ana",
  },
  attachments: [],
};

describe("IssueTable", () => {
  it("calls priority toggle when clicking the priority badge", async () => {
    const user = userEvent.setup();
    const onTogglePriority = vi.fn();

    render(
      <MemoryRouter>
        <IssueTable
          issues={[issue]}
          onDelete={vi.fn()}
          onTogglePriority={onTogglePriority}
          sortKey="title"
          sortDirection="asc"
          onSort={vi.fn()}
        />
      </MemoryRouter>
    );

    await user.click(
      screen.getByRole("button", {
        name: /high/i,
      })
    );

    expect(onTogglePriority)
      .toHaveBeenCalledWith(issue);
  });
});
