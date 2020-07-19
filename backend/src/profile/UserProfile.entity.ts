import { Field, ID, ObjectType, HideField } from '@nestjs/graphql';
import {
    Column, Entity,
    PrimaryGeneratedColumn
} from 'typeorm';
import { Point } from 'geojson';
import { CyclingType } from './CyclingType.enum';


@Entity('user_profile')
@ObjectType('UserProfile')
export class UserProfile {

    constructor(userId: string, name: string, placeName: string, exactLocation: Point, preferredCyclingTypes: CyclingType[]) {
        this.userId = userId;
        this.placeName = placeName;
        this.name = name;
        this.exactLocation = exactLocation;
        this.preferredCyclingTypes = preferredCyclingTypes;
    }

    @PrimaryGeneratedColumn({ name: "user_id" })
    @Field(type => ID)
    userId: string;

    @Column({ nullable: false })
    name: string;

    @Column({ nullable: false, name: "place_name" })
    placeName: string;

    @Column("geography", {
        spatialFeatureType: "Point",
        srid: 4326,
        nullable: false,
        name: "exact_location"
    })
    @HideField()
    exactLocation: Point

    @Column("int", { nullable: false, name: "preferred_cycling_types", array: true })
    preferredCyclingTypes: CyclingType[];

    @Column({ nullable: false, name: "created_at" })
    createdAt: Date;

    @Column({ nullable: false, name: "updated_at" })
    updatedAt: Date;
}
