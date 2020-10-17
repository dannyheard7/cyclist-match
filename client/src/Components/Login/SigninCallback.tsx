import ky from "ky";
import React, { useEffect } from "react";
import { QueryStatus, useMutation } from "react-query";
import { useHistory } from "react-router-dom";
import { useApi } from "../../Hooks/Api";
import { useAuthentication } from "../Authentication/AuthenticationContext";
import Loading from "../Loading/Loading";

const LoginCallback: React.FC = () => {
  const { loading, signinCallback, isAuthenticated } = useAuthentication();
  const api = useApi();
  const [mutate, { status, data, error }] = useMutation(() => api.post("http://localhost:5000/auth/login").json<{ hasProfile: boolean }>())

  const { push } = useHistory();

  useEffect(() => {
    if (!loading && !isAuthenticated) signinCallback();
  }, [isAuthenticated, loading, signinCallback]);

  useEffect(() => {
    if (isAuthenticated) mutate()
  }, [isAuthenticated])


  useEffect(() => {
    if (data) {
      if (data.hasProfile) push("/dashboard");
      else push("profile/create");
    }
  }, [data]);

  if (loading || status === QueryStatus.Loading) return <Loading />;
  else if (status == QueryStatus.Error) return <p>Sorry, an error occured</p>;



  return null;
};

export default LoginCallback;
