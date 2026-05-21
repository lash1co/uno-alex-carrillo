export const IssuePriority = {
  LOW: 0,
  MEDIUM: 1,
  HIGH: 2,
} as const;

export type IssuePriority =
  typeof IssuePriority[keyof typeof IssuePriority];

export const IssueStatus = {
  OPEN: 0,
  IN_PROGRESS: 1,
  CLOSED: 2,
} as const;

export type IssueStatus =
  typeof IssueStatus[keyof typeof IssueStatus];

export interface Attachment {
  id: string;
  fileName: string;
  fileUrl: string;
}

export interface IssueAssignee {
  id: string;
  name: string;
  email?: string;
}

export interface Issue {
  id: string;
  title: string;
  description?: string;
  priority: IssuePriority;
  status: IssueStatus;
  assigneeId?: string;
  assignee?: IssueAssignee;
  attachments: Attachment[];
}

export interface CreateIssueRequest {
  title: string;
  description: string;
  priority: IssuePriority;
}

export interface UpdateIssueRequest {
  status: IssueStatus;
  priority: IssuePriority;
  assigneeId?: string | null;
}
