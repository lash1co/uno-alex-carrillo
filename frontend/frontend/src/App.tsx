import { ToastContainer } from "react-toastify";
import { AppRouter } from "./router/AppRouter";

import "react-toastify/dist/ReactToastify.css";
import "./styles/globals.css";
import "./styles/layout.css";
import "./styles/pages.css";
import "./styles/forms.css";
import "./components/issues/issues.css";
import "./components/attachments/attachments.css";

function App() {
  return (
    <>
      <AppRouter />

      <ToastContainer
        position="top-right"
      />
    </>
  );
}

export default App;
