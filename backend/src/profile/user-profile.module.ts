import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { UserProfileResolver } from './user-profile.resolver';
import { UserProfileService } from './user-profile.service';
import { UserProfile } from './UserProfile.entity';

@Module({
  imports: [
    TypeOrmModule.forFeature([UserProfile])
  ],
  providers: [UserProfileService, UserProfileResolver],
  exports: []
})
export class UserProfileModule { }
