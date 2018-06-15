extern "C"
{
    double CheckFreeSpace() {
        uint64_t totalFreeSpaceInUnsignedInts = 0;
        NSError *error = nil;
        NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
        NSDictionary *dictionary = [[NSFileManager defaultManager] attributesOfFileSystemForPath:[paths lastObject] error: &error];
        if (dictionary) {
            NSNumber *freeFileSystemSizeInBytes = [dictionary objectForKey:NSFileSystemFreeSize];
            totalFreeSpaceInUnsignedInts = [freeFileSystemSizeInBytes unsignedLongLongValue];
        }
        else {
            NSLog(@"Error Obtaining System Memory Info: Domain = %@, Code = %ld", [error domain], (long)[error code]);
            return 0.0;
        }
        return totalFreeSpaceInUnsignedInts;
    }
    
}