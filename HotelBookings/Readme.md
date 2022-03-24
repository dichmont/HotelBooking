# Booking Manager Technical Specification

## Booking storage
### Database
In practice, using a database to persist bookings would be preferable as it assists with durability (resilience to program closure, power failure etc.). Additionally, transactions abstract & simplify thread safety (ie unique constraints* would prevent concurrent inserts with the same date and room number). However, this option provides less opportunity to demonstrate unit testing & TDD.

Could introduce a repository interface to allow for replacement with the better database implementation, but it would necessitate a DI container and leave BookingManager as mostly boiler plate.

*primary key in practice for lookup performance as will be querying against date & room number for IsRoomAvailable.

### Concurrent Dictionary
A dictionary was instead used as an in-memory booking store.
- Concurrent for thread safety. 
- Dictionary as a booking can be conveniently represented as a key-value: date & room number as keys; surname as values.

## Concurrent Dictionary key
A booking is identified by two characteristics - date & room number. Therefore it is necessary that both of these values form the key of the dictionary. Several implementations were considered for combining these keys for the dictionary.

### Nested dictionaries
Unnecessary memory overhead

### String concatenation 
Slight risk of key collision depending on ToString implementations. Not a well defined variable as actually a combination of two variables.

### Hashing values
Unnecessary performance hit

### Class to combine values
Unnecessarily overcomplicated - need to override Equals & GetHashCode to ensure comparison by reference

### Named struct
Chosen implementation. Value type so default equivalence trivial. Named properties for readability. Immutable as does not need to be edited. Struct over tuple for definition reuse (ie  across methods & dictionary).

## Preventing time components in booking date 

Because using dictionary to prevent multiple room bookings for the same day, important that bookings on the same day have the same key value. However, different times could lead to multiple successful bookings on the same day. This is prevented by ensuring time is midday (not sure which day midnight is on). Alternatively could sanitise inputs before insertion.

## getAvailableRooms

Could reimplement the booking dictionary as nested dictionaries to allow O(1) lookup of bookings both by room and date & date alone, however this could have thread safety implications & significant memory use. Opted to instead O(N) iterate over all existing bookings. This can be optimised by removing expired bookings, or moving to database implementation which can use primary key lookups if the definition is across (date, room)*
* in that order.

