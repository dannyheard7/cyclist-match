import Availability from '../Enums/Availability';
import CyclingType from '../Enums/CyclingType';
import { User } from './User';

export default interface Profile extends User {
    displayName: string;
    locationName: string;
    cyclingTypes: Array<CyclingType>;
    availability: Array<Availability>;
    minDistance: number;
    maxDistance: number;
    speed: number;
    distanceFromUserKM: number;
    profileImage?: string;
}
