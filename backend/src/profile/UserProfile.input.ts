import { Field, Float, InputType } from '@nestjs/graphql';
import { CyclingType } from './CyclingType.enum';

@InputType()
export class UserProfileInput {
    name: string;

    placeName: string;

    @Field(type => Float)
    exactLocationLongitude: number;

    @Field(type => Float)
    exactLocationLatitude: number;

    preferredCyclingTypes: CyclingType[];
}
