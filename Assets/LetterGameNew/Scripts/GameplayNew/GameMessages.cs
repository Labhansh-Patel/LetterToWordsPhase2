namespace Gameplay
{
    public static class GameMessages
    {
        public const string CentreTileMissing = "The first turn must use the center space";
        public static string InvalidConnection = "Invalid Connection";
        public static string InvalidWord = "Please Make Valid Words";
        public static string CannotFindPlayers = "Cannot Find Players for game please try again later";
        public static string InsufficentBonus = "Insufficient BonusPoints";
        public static string NoAvailableTiles = "No Free Tiles To Replace";
        public static string PowerUpOnlyUsableInMultiplayer = "Cannot use this Power-Up right now";
        public static string ExtraTimeBlocked = "Extra Time is blocked for this round";
        public static string CannotUseBonusTwice = "Cannot use same bonus twice in round";
        public static string Disconnected = "Oops ! It seems like you have been disconnected";
        public static string AllPlayersDisconnected = "All the other players have been disconnected!";
        public static string AlreadyBlockedForRound = "Powerup is already blocked for this round";
        public static string CannotFindRoom = "Cannot find the room";
        public static string EnterRoomCode = "Please Enter Room Code";
        public static string CannotCreateRoomNonPremium = "Please purchase premium for creating private Games";
        public static string CannotJoinPrivateRoomNonPremium = "Please purchase premium for joining private Games";
        public static string JoinMembershipRequestFailed = "Membership request failed. Please try again later";
        public static string CancleMembershipRequestFailed = "Cancelation request failed. Please try again later";
        public static string GameCompleted = "The Game has been Completed";
        public static string PleaseAddATile = "Please add a tile to submit your score";
    }
}