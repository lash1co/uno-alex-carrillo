import { useForm } from "react-hook-form";
import {
  IssuePriority,
  type CreateIssueRequest,
} from "../../types/issue";

type Props = {
  onCancel: () => void;
  onSubmit: (data: CreateIssueRequest) => Promise<void>;
};

export const CreateIssueForm = ({
  onCancel,
  onSubmit,
}: Props) => {
  const {
    register,
    handleSubmit,
    formState: {
      errors,
      isValid,
      isSubmitting,
    },
  } = useForm<CreateIssueRequest>({
    mode: "onChange",
    defaultValues: {
      title: "",
      description: "",
      priority: IssuePriority.LOW,
    },
  });

  return (
    <form
      className="form"
      onSubmit={handleSubmit(onSubmit)}
    >
      <div className="form-field">
        <label htmlFor="issue-title">
          Title
        </label>
        <input
          id="issue-title"
          {...register("title", {
            required: "Title is required",
            minLength: {
              value: 3,
              message:
                "Title must have at least 3 characters",
            },
          })}
        />
        {errors.title && (
          <p className="form-error">
            {errors.title.message}
          </p>
        )}
      </div>

      <div className="form-field">
        <label htmlFor="issue-description">
          Description
        </label>
        <textarea
          id="issue-description"
          rows={4}
          {...register("description", {
            required:
              "Description is required",
            minLength: {
              value: 5,
              message:
                "Description must have at least 5 characters",
            },
          })}
        />
        {errors.description && (
          <p className="form-error">
            {errors.description.message}
          </p>
        )}
      </div>

      <div className="form-field">
        <label htmlFor="issue-priority">
          Priority
        </label>
        <select
          id="issue-priority"
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

      <div className="form-actions">
        <button
          className="button button--secondary"
          type="button"
          onClick={onCancel}
        >
          Cancel
        </button>
        <button
          type="submit"
          disabled={!isValid || isSubmitting}
        >
          Create issue
        </button>
      </div>
    </form>
  );
};
