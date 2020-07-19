import { gql } from '@apollo/client';
import { Profile } from '../../Common/Interfaces/Profile';
import CyclingType from '../../Common/Enums/CyclingType';

export const CREATE_PROFILE_MUTATION = gql`
  mutation createUserProfile($profile: UserProfileInput!) {
    createUserProfile(userProfile: $profile) {
      name
      placeName
      preferredCyclingTypes
      createdAt
    }
  }
`;

export default interface CreateProfileInput {
  exactLocationLongitude: number,
  exactLocationLatitude: number,
  name: string,
  placeName: string,
  preferredCyclingTypes: CyclingType[]
}

export interface CreateProfileMutationVars {
  profile: CreateProfileInput;
}

export interface CreateProfileMutationData {
  createProfile: Profile
}