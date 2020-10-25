import CyclingType from "../Enums/CyclingType";

export interface Profile {
    givenName: string,
    familyName: string,
    placeName: string
    preferredCyclingTypes: CyclingType[]
    picture?: string,
    createdAt: Date
}