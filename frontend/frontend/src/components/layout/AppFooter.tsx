export const AppFooter = () => {
  const year = new Date().getFullYear();

  return (
    <footer className="app-footer">
      <span>Issue Tracker</span>
      <span>v1.0.0</span>
      <span>{year}</span>
    </footer>
  );
};
