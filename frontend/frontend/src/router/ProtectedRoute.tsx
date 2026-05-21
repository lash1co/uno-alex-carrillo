import { Navigate } from "react-router-dom";
import { AppFooter } from "../components/layout/AppFooter";
import { Navbar } from "../components/layout/Navbar";

type Props = {
  children: React.ReactNode;
};

export const ProtectedRoute = ({
  children,
}: Props) => {
  const token =
    localStorage.getItem("token");

  if (!token) {
    return (
      <Navigate to="/login" />
    );
  }

  return (
    <div className="app-shell">
      <Navbar />
      <main className="app-main">
        {children}
      </main>
      <AppFooter />
    </div>
  );
};
