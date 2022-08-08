if not exists (select 1 from dbo.[Teams])

begin
    insert into dbo.[Teams] (TeamName, City, Stadium, Wins, Losses, Draws, Symbol, Division, Conference)
        values ('Bengals', 'Cincinnati', 'Paul Brown Stadium', '', '', '', 'CIN', 'North', 'AFC'),
               ('Raiders', 'Las Vegas', 'Allegiant Stadium', '', '', '', 'LV', 'West', 'AFC'),
               ('Chiefs', 'Kansas City', 'Arrowhead Stadium', '', '', '', 'KC', 'West', 'AFC'),
               ('Cowboys', 'Dallas', 'AT&T Stadium', '', '', '', 'DAL', 'East', 'NFC'),
               ('Panthers', 'Carolina', 'Bank Of America Stadium', '', '', '', 'CAR', 'South', 'NFC'),
               ('Saints', 'New Orleans', 'Caesars Superdome', '', '', '', 'NO', 'South', 'NFC'),
               ('Broncos', 'Denver', 'Mile High Stadium', '', '', '', 'DEN', 'West', 'AFC'),
               ('Commanders', 'Washington', 'FedEx Field', '', '', '', 'WAS', 'East', 'NFC'),
               ('Browns', 'Cleveland', 'First Energy Stadium', '', '', '', 'CLE', 'North', 'AFC'),
               ('Lions', 'Detroit', 'Ford Field', '', '', '', 'DET', 'North', 'NFC'),
               ('Patriots', 'New England', 'Gillette Stadium', '', '', '', 'NE', 'East', 'AFC'),
               ('Dolphins', 'Miami', 'Hard Rock Stadium', '', '', '', 'MIA', 'East', 'AFC'),
               ('Bills', 'Buffalo', 'Highmark Stadium', '', '', '', 'BUF', 'East', 'AFC'),
               ('Packers', 'Green Bay', 'Lambeau Field', '', '', '', 'GB', 'North', 'NFC'),
               ('49ers', 'San Francisco', 'Levi''s Stadium', '', '', '', 'SF', 'West', 'NFC'),
               ('Eagles', 'Philadelphia', 'Lincoln Financial Field', '', '', '', 'PHI', 'East', 'NFC'),
               ('Colts', 'Indianapolis', 'Lucas Oil Stadium', '', '', '', 'IND', 'South', 'AFC'),
               ('Seahawks', 'Seattle', 'Lumen Field', '', '', '', 'SEA', 'West', 'NFC'),
               ('Ravens', 'Baltimore', 'M&T Bank Stadium', '', '', '', 'BAL', 'North', 'AFC'),
               ('Falcons', 'Atlanta', 'Mercedes-Benz Stadium', '', '', '', 'ATL', 'South', 'NFC'),
               ('Giants', 'New York', 'Met Life Stadium', '', '', '', 'NYG', 'East', 'NFC'),
               ('Jets', 'New York', 'Met Life Stadium', '', '', '', 'NYJ', 'East', 'AFC'),
               ('Titans', 'Tennessee', 'Nissan Stadium', '', '', '', 'TEN', 'South', 'AFC'),
               ('Texans', 'Houston', 'NRG Stadium', '', '', '', 'HOU', 'South', 'AFC'),
               ('Buccaneers', 'Tampa Bay', 'Raymond James Stadium', '', '', '', 'TB', 'South', 'NFC'),
               ('Rams', 'Los Angeles', 'SoFi Stadium', '', '', '', 'LAR', 'West', 'NFC'),
               ('Chargers', 'Los Angeles', 'SoFi Stadium', '', '', '', 'LAC', 'West', 'AFC'),
               ('Bears', 'Chicago', 'Soldier Field', '', '', '', 'CHI', 'North', 'NFC'),
               ('Cardinals', 'Arizona', 'State Farm Stadium', '', '', '', 'ARI', 'West', 'NFC'),
               ('Jaguars', 'Jacksonville', 'TIAA Bank Field', '', '', '', 'JAX', 'South', 'AFC'),
               ('Vikings', 'Minnesota', 'U.S. Bank Stadium', '', '', '', 'MIN', 'North', 'NFC'),
               ('Steelers', 'Pittsburgh', 'Heinz Field', '', '', '', 'PIT', 'North', 'AFC')

    insert into dbo.[HouseAccount] (AccountBalance)
        values (500000);

end
