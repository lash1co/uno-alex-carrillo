import { useEffect, useState } from "react";
import { assigneeService } from "../services/assigneeService";
import type { Assignee } from "../types/assignee";

export const useAssignees = () => {
  const [assignees, setAssignees] =
    useState<Assignee[]>([]);

  const [loading, setLoading] =
    useState(true);

  useEffect(() => {
    loadAssignees();
  }, []);

  const loadAssignees = async () => {
    try {
      setLoading(true);

      const response =
        await assigneeService.getAll();

      setAssignees(response);
    } finally {
      setLoading(false);
    }
  };

  return {
    assignees,
    loading,
  };
};
