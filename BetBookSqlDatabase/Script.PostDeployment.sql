if not exists (select 1 from dbo.[Teams])

begin
    insert into dbo.[Teams] (TeamName, City, Stadium, Wins, Losses, Draws)
        values ('Bengals', 'Cincinnati', 'Paul Brown Stadium', '', '', ''),
               ('Raiders', 'Las Vegas', 'Allegiant Stadium', '', '', ''),
               ('Chiefs', 'Kansas City', 'Arrowhead Stadium', '', '', ''),
               ('Cowboys', 'Dallas', 'AT&T Stadium', '', '', ''),
               ('Panthers', 'Carolina', 'Bank Of America Stadium', '', '', ''),
               ('Saints', 'New Orleans', 'Caesars Superdome', '', '', ''),
               ('Broncos', 'Denver', 'Mile High Stadium', '', '', ''),
               ('Commanders', 'Washington', 'FedEx Field', '', '', ''),
               ('Browns', 'Cleveland', 'First Energy Stadium', '', '', ''),
               ('Lions', 'Detroit', 'Ford Field', '', '', ''),
               ('Patriots', 'New England', 'Gillette Stadium', '', '', ''),
               ('Dolphins', 'Miami', 'Hard Rock Stadium', '', '', ''),
               ('Bills', 'Buffalo', 'Highmark Stadium', '', '', ''),
               ('Packers', 'Green Bay', 'Lambeau Field', '', '', ''),
               ('49ers', 'San Francisco', 'Levi''s Stadium', '', '', ''),
               ('Eagles', 'Philadelphia', 'Lincoln Financial Field', '', '', ''),
               ('Colts', 'Indianapolis', 'Lucas Oil Stadium', '', '', ''),
               ('Seahawks', 'Seattle', 'Lumen Field', '', '', ''),
               ('Ravens', 'Baltimore', 'M&T Bank Stadium', '', '', ''),
               ('Falcons', 'Atlanta', 'Mercedes-Benz Stadium', '', '', ''),
               ('Giants', 'New York', 'Met Life Stadium', '', '', ''),
               ('Jets', 'New York', 'Met Life Stadium', '', '', ''),
               ('Titans', 'Tennessee', 'Nissan Stadium', '', '', ''),
               ('Texans', 'Houston', 'NRG Stadium', '', '', ''),
               ('Buccaneers', 'Tampa Bay', 'Raymond James Stadium', '', '', ''),
               ('Rams', 'Los Angeles', 'SoFi Stadium', '', '', ''),
               ('Chargers', 'Los Angeles', 'SoFi Stadium', '', '', ''),
               ('Bears', 'Chicago', 'Soldier Field', '', '', ''),
               ('Cardinals', 'Arizona', 'State Farm Stadium', '', '', ''),
               ('Jaguars', 'Jacksonville', 'TIAA Bank Field', '', '', ''),
               ('Vikings', 'Minnesota', 'U.S. Bank Stadium', '', '', ''),
               ('Steelers', 'Pittsburgh', 'Heinz Field', '', '', '')

    insert into dbo.[HouseAccount] (AccountBalance)
        values (500000);

end
