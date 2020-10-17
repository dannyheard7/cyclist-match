import React, { useEffect } from "react";
import { useHistory } from "react-router-dom";
import { useAuthentication } from "../Authentication/AuthenticationContext";

const LoginCallback: React.FC = () => {
  const { loading, signinCallback, isAuthenticated } = useAuthentication();
  const { push } = useHistory();

  useEffect(() => {
    if (isAuthenticated) push("/dashboard")
  }, [isAuthenticated, push])

  useEffect(() => {
    if (!loading && !isAuthenticated) signinCallback();
  }, [isAuthenticated, loading, signinCallback])

  return null;
};

export default LoginCallback;
