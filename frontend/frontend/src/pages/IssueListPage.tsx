import { useMemo, useState } from "react";
import { toast } from "react-toastify";
import { CreateIssueForm } from "../components/issues/CreateIssueForm";
import {
  IssueTable,
  type SortDirection,
  type SortKey,
} from "../components/issues/IssueTable";
import { useIssues } from "../hooks/useIssues";
import { issueService } from "../services/issueService";
import type {
  CreateIssueRequest,
  Issue,
} from "../types/issue";
import {
  filterAndSortIssues,
  getNextPriority,
} from "../utils/issueTableUtils";

export const IssueListPage = () => {
  const {
    data,
    loading,
    page,
    setPage,
    refresh,
  } = useIssues();

  const [
    isCreateModalOpen,
    setIsCreateModalOpen,
  ] = useState(false);

  const [
    issueToDelete,
    setIssueToDelete,
  ] = useState<Issue | null>(null);

  const [searchTerm, setSearchTerm] =
    useState("");

  const [sortKey, setSortKey] =
    useState<SortKey>("title");

  const [sortDirection, setSortDirection] =
    useState<SortDirection>("asc");

  const currentPage =
    data?.page && data.page > 0
      ? data.page
      : 1;

  const totalPages =
    data?.totalPages && data.totalPages > 0
      ? data.totalPages
      : 1;

  const visibleIssues = useMemo(() => {
    const normalizedSearch =
      searchTerm.trim().toLowerCase();

    const items = data?.items || [];

    return filterAndSortIssues(
      items,
      normalizedSearch,
      sortKey,
      sortDirection
    );
  }, [
    data?.items,
    searchTerm,
    sortDirection,
    sortKey,
  ]);

  if (loading) {
    return (
      <div className="container">
        <p className="loading-state">
          Loading issues...
        </p>
      </div>
    );
  }

  const handleCreateIssue = async (
    values: CreateIssueRequest
  ) => {
    await issueService.create(values);
    toast.success("Issue created successfully");
    setIsCreateModalOpen(false);
    await refresh();
  };

  const handleDeleteIssue = async () => {
    if (!issueToDelete) return;

    await issueService.delete(
      issueToDelete.id
    );

    toast.success("Issue deleted successfully");
    setIssueToDelete(null);
    await refresh();
  };

  const handleSort = (key: SortKey) => {
    if (key === sortKey) {
      setSortDirection((current) =>
        current === "asc" ? "desc" : "asc"
      );
      return;
    }

    setSortKey(key);
    setSortDirection("asc");
  };

  const handleTogglePriority = async (
    issue: Issue
  ) => {
    const nextPriority =
      getNextPriority(issue.priority);

    refresh({
      updater: (currentData) => ({
        ...currentData,
        items: currentData.items.map(
          (currentIssue) =>
            currentIssue.id === issue.id
              ? {
                  ...currentIssue,
                  priority: nextPriority,
                }
              : currentIssue
        ),
      }),
    });

    try {
      await issueService.update(issue.id, {
        status: issue.status,
        priority: nextPriority,
        assigneeId: issue.assigneeId || null,
      });

      toast.success("Priority updated successfully");
    } catch {
      refresh({
        updater: (currentData) => ({
          ...currentData,
          items: currentData.items.map(
            (currentIssue) =>
              currentIssue.id === issue.id
                ? issue
                : currentIssue
          ),
        }),
      });
    }
  };

  return (
    <div className="container">
      <div className="page-card">
        <div className="page-header">
          <div>
            <h1>Issues</h1>
            <p className="page-subtitle">
              Review, prioritize, and open issue details.
            </p>
          </div>

          <button
            type="button"
            onClick={() =>
              setIsCreateModalOpen(true)
            }
          >
            New issue
          </button>
        </div>

        <div className="table-toolbar">
          <label
            className="search-field"
            htmlFor="issue-search"
          >
            <span>Search</span>
            <input
              id="issue-search"
              type="search"
              placeholder="Search visible columns"
              value={searchTerm}
              onChange={(event) =>
                setSearchTerm(
                  event.target.value
                )
              }
            />
          </label>
        </div>

        <IssueTable
          issues={visibleIssues}
          onDelete={setIssueToDelete}
          onTogglePriority={handleTogglePriority}
          sortKey={sortKey}
          sortDirection={sortDirection}
          onSort={handleSort}
        />

        <div className="pagination">
          <button
            disabled={
              !data?.hasPreviousPage
            }
            onClick={() =>
              setPage(page - 1)
            }
          >
            Previous
          </button>

          <span>
            Page {currentPage} of{" "}
            {totalPages}
          </span>

          <button
            disabled={
              !data?.hasNextPage
            }
            onClick={() =>
              setPage(page + 1)
            }
          >
            Next
          </button>
        </div>
      </div>

      {isCreateModalOpen && (
        <div className="modal-backdrop">
          <div
            className="modal"
            role="dialog"
            aria-modal="true"
            aria-labelledby="create-issue-title"
          >
            <div className="modal-header">
              <div>
                <h2 id="create-issue-title">
                  Create issue
                </h2>
                <p>
                  Add the initial details and priority.
                </p>
              </div>
            </div>

            <CreateIssueForm
              onCancel={() =>
                setIsCreateModalOpen(false)
              }
              onSubmit={handleCreateIssue}
            />
          </div>
        </div>
      )}

      {issueToDelete && (
        <div className="modal-backdrop">
          <div
            className="modal modal--compact"
            role="dialog"
            aria-modal="true"
            aria-labelledby="delete-issue-title"
          >
            <div className="modal-header">
              <div>
                <h2 id="delete-issue-title">
                  Delete issue
                </h2>
                <p>
                  This action cannot be undone.
                </p>
              </div>
            </div>

            <p className="modal-copy">
              Are you sure you want to delete{" "}
              <strong>
                {issueToDelete.title}
              </strong>
              ?
            </p>

            <div className="form-actions">
              <button
                className="button button--secondary"
                type="button"
                onClick={() =>
                  setIssueToDelete(null)
                }
              >
                Cancel
              </button>
              <button
                className="button button--danger"
                type="button"
                onClick={handleDeleteIssue}
              >
                Delete issue
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
