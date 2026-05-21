import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { authService } from "../services/authService";
import type { LoginRequest } from "../types/auth";

export const LoginPage = () => {
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: {
      errors,
      isValid,
      isSubmitting,
    },
  } = useForm<LoginRequest>({
    mode: "onChange",
  });

  const onSubmit = async (
    data: LoginRequest
  ) => {
    const response =
      await authService.login(data);

    localStorage.setItem(
      "token",
      response.token
    );

    localStorage.setItem(
      "user",
      JSON.stringify(response.user)
    );

    navigate("/issues");
  };

  return (
    <div className="login-page">
      <div className="page-card login-card">
        <div className="page-header">
          <div>
            <h1>Login</h1>
            <p className="page-subtitle">
              Access the issue tracker dashboard.
            </p>
          </div>
        </div>

        <form
          className="form form--narrow"
          onSubmit={handleSubmit(onSubmit)}
        >
          <div className="form-field">
            <label htmlFor="login-email">
              Email
            </label>

            <input
              id="login-email"
              type="email"
              {...register("email", {
                required: "Email is required",
                pattern: {
                  value:
                    /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                  message:
                    "Enter a valid email address",
                },
              })}
            />

            {errors.email && (
              <p className="form-error">
                {errors.email.message}
              </p>
            )}
          </div>

          <div className="form-field">
            <label htmlFor="login-password">
              Password
            </label>

            <input
              id="login-password"
              type="password"
              {...register("password", {
                required:
                  "Password is required",
              })}
            />

            {errors.password && (
              <p className="form-error">
                {errors.password.message}
              </p>
            )}
          </div>

          <button
            type="submit"
            disabled={!isValid || isSubmitting}
          >
            Login
          </button>
        </form>
      </div>
    </div>
  );
};
