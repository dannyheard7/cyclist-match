import React, { useEffect } from "react";
import { useMutation } from "react-query";
import { useHistory } from "react-router-dom";
import { useAuthentication } from "../../Components/Authentication/AuthenticationContext";
import ErrorMessage from "../../Components/ErrorMessage/ErrorMessage";
import Loading from "../../Components/Loading/Loading";
import { useApi } from "../../Hooks/useApi";

const LoginCallback: React.FC = () => {
  const { loading, signinCallback, isAuthenticated } = useAuthentication();
  const api = useApi();
  const { mutate, isLoading, data, isError } = useMutation(() => api.post("auth/login").json<{ hasProfile: boolean }>())

  const { push } = useHistory();

  useEffect(() => {
    if (!loading && !isAuthenticated) signinCallback();
  }, [isAuthenticated, loading, signinCallback]);

  useEffect(() => {
    if (isAuthenticated) mutate()
  }, [isAuthenticated, mutate])

  useEffect(() => {
    if (data) {
      if (data.hasProfile) push("/dashboard");
      else push("profile/create");
    }
  }, [data, push]);

  if (loading || isLoading) return <Loading />;
  else if (isError) return <ErrorMessage />;

  return null;
};

export default LoginCallback;
