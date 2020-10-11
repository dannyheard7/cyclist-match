import React from "react";
import { useAuthentication } from "../Authentication/AuthenticationContext";


const Dashboard: React.FC = () => {
  const { user } = useAuthentication();

  return (
    <p>
      Hello {user?.profile.given_name} {user?.profile.family_name}
    </p>
  );
};

export default Dashboard;
