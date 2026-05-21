import { useNavigate } from "react-router-dom";

export const Navbar = () => {
  const navigate = useNavigate();

  const user = JSON.parse(
    localStorage.getItem("user") || "{}"
  );

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");

    navigate("/login");
  };

  return (
    <nav className="app-nav">
      <div className="app-nav__brand">
        <span className="app-nav__mark">
          IT
        </span>
        <span>
          Issue Tracker
        </span>
      </div>

      <div className="app-nav__actions">
        <span className="app-nav__user">
          Welcome {user.name}
        </span>

        <button
          onClick={handleLogout}
        >
          Logout
        </button>
      </div>
    </nav>
  );
};
