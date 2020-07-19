import { gql } from '@apollo/client';
import { Profile } from '../../Common/Interfaces/Profile';

export const PROFILE_QUERY = gql`
  query Profile($id: ID!) {
    profile(id: $id) {
      name
      placeName
      preferredCyclingTypes
      createdAt
    }
  }
`;

export interface ProfileQueryVars {
  id: string;
}

export interface ProfileQueryData {
  profile: Profile
}