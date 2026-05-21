import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";
import { LoginPage } from "../pages/LoginPage";
import { IssueListPage } from "../pages/IssueListPage";
import { IssueDetailPage } from "../pages/IssueDetailPage";
import { ProtectedRoute } from "./ProtectedRoute";

export const AppRouter = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/login"
          element={<LoginPage />}
        />

        <Route
          path="/issues"
          element={
            <ProtectedRoute>
              <IssueListPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/issues/:id"
          element={
            <ProtectedRoute>
              <IssueDetailPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="*"
          element={
            <Navigate to="/issues" />
          }
        />
      </Routes>
    </BrowserRouter>
  );
};