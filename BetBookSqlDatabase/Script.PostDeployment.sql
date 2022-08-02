if not exists (select 1 from dbo.[Teams])

begin
    insert into dbo.[Teams] (TeamName, City, Stadium, Wins, Losses, Draws, Symbol)
        values ('Bengals', 'Cincinnati', 'Paul Brown Stadium', '', '', '', 'CIN'),
               ('Raiders', 'Las Vegas', 'Allegiant Stadium', '', '', '', 'LV'),
               ('Chiefs', 'Kansas City', 'Arrowhead Stadium', '', '', '', 'KC'),
               ('Cowboys', 'Dallas', 'AT&T Stadium', '', '', '', 'DAL'),
               ('Panthers', 'Carolina', 'Bank Of America Stadium', '', '', '', 'CAR'),
               ('Saints', 'New Orleans', 'Caesars Superdome', '', '', '', 'NO'),
               ('Broncos', 'Denver', 'Mile High Stadium', '', '', '', 'DEN'),
               ('Commanders', 'Washington', 'FedEx Field', '', '', '', 'WAS'),
               ('Browns', 'Cleveland', 'First Energy Stadium', '', '', '', 'CLE'),
               ('Lions', 'Detroit', 'Ford Field', '', '', '', 'DET'),
               ('Patriots', 'New England', 'Gillette Stadium', '', '', '', 'NE'),
               ('Dolphins', 'Miami', 'Hard Rock Stadium', '', '', '', 'MIA'),
               ('Bills', 'Buffalo', 'Highmark Stadium', '', '', '', 'BUF'),
               ('Packers', 'Green Bay', 'Lambeau Field', '', '', '', 'GB'),
               ('49ers', 'San Francisco', 'Levi''s Stadium', '', '', '', 'SF'),
               ('Eagles', 'Philadelphia', 'Lincoln Financial Field', '', '', '', 'PHI'),
               ('Colts', 'Indianapolis', 'Lucas Oil Stadium', '', '', '', 'IND'),
               ('Seahawks', 'Seattle', 'Lumen Field', '', '', '', 'SEA'),
               ('Ravens', 'Baltimore', 'M&T Bank Stadium', '', '', '', 'BAL'),
               ('Falcons', 'Atlanta', 'Mercedes-Benz Stadium', '', '', '', 'ATL'),
               ('Giants', 'New York', 'Met Life Stadium', '', '', '', 'NYG'),
               ('Jets', 'New York', 'Met Life Stadium', '', '', '', 'NYJ'),
               ('Titans', 'Tennessee', 'Nissan Stadium', '', '', '', 'TEN'),
               ('Texans', 'Houston', 'NRG Stadium', '', '', '', 'HOU'),
               ('Buccaneers', 'Tampa Bay', 'Raymond James Stadium', '', '', '', 'TB'),
               ('Rams', 'Los Angeles', 'SoFi Stadium', '', '', '', 'LAR'),
               ('Chargers', 'Los Angeles', 'SoFi Stadium', '', '', '', 'LAC'),
               ('Bears', 'Chicago', 'Soldier Field', '', '', '', 'CHI'),
               ('Cardinals', 'Arizona', 'State Farm Stadium', '', '', '', 'ARI'),
               ('Jaguars', 'Jacksonville', 'TIAA Bank Field', '', '', '', 'JAX'),
               ('Vikings', 'Minnesota', 'U.S. Bank Stadium', '', '', '', 'MIN'),
               ('Steelers', 'Pittsburgh', 'Heinz Field', '', '', '', 'PIT')

    insert into dbo.[HouseAccount] (AccountBalance)
        values (500000);

end
