import React, { Fragment } from "react";
import { useAuthentication } from "../../Components/Authentication/AuthenticationContext";
import ProfileMatches from "../../Components/ProfileMatches/ProfileMatches";


const Dashboard: React.FC = () => {
  const { user } = useAuthentication();

  return (
    <Fragment>
      <p>
        Hello {user?.profile.given_name} {user?.profile.family_name}
      </p>
      <ProfileMatches />
    </Fragment>
  );
};

export default Dashboard;
