import {
  render,
  screen,
} from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import {
  describe,
  expect,
  it,
  vi,
} from "vitest";
import { CreateIssueForm } from "./CreateIssueForm";

describe("CreateIssueForm", () => {
  it("keeps submit disabled until title and description are valid", async () => {
    const user = userEvent.setup();
    const onSubmit = vi.fn();

    render(
      <CreateIssueForm
        onCancel={vi.fn()}
        onSubmit={onSubmit}
      />
    );

    const submitButton = screen.getByRole(
      "button",
      { name: /create issue/i }
    );

    expect(submitButton).toBeDisabled();

    await user.type(
      screen.getByLabelText(/title/i),
      "Bug"
    );
    await user.type(
      screen.getByLabelText(/description/i),
      "Broken"
    );

    expect(submitButton).toBeEnabled();
  });
});
