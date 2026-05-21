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
import { LoginPage } from "./LoginPage";

vi.mock("../services/authService", () => ({
  authService: {
    login: vi.fn(),
  },
}));

describe("LoginPage", () => {
  it("requires a valid email before enabling submit", async () => {
    const user = userEvent.setup();

    render(
      <MemoryRouter>
        <LoginPage />
      </MemoryRouter>
    );

    const submitButton = screen.getByRole(
      "button",
      { name: /login/i }
    );

    expect(submitButton).toBeDisabled();

    await user.type(
      screen.getByLabelText(/email/i),
      "not-an-email"
    );
    await user.type(
      screen.getByLabelText(/password/i),
      "admin123"
    );

    expect(
      await screen.findByText(
        /enter a valid email address/i
      )
    ).toBeInTheDocument();
    expect(submitButton).toBeDisabled();
  });
});
