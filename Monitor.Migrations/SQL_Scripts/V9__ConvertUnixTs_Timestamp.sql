alter table "Messages"
    rename column "ConsumerServerRequestInTs" to "ConsumerServerRequestIn";

alter table "Messages"
    rename column "ConsumerServerRequestOutTs" to "ConsumerServerRequestOut";

alter table "Messages"
    rename column "ConsumerServerResponseInTs" to "ConsumerServerResponseIn";

alter table "Messages"
    rename column "ConsumerServerResponseOutTs" to "ConsumerServerResponseOut";

alter table "Messages"
    rename column "ProducerServerRequestInTs" to "ProducerServerRequestIn";

alter table "Messages"
    rename column "ProducerServerRequestOutTs" to "ProducerServerRequestOut";

alter table "Messages"
    rename column "ProducerServerResponseInTs" to "ProducerServerResponseIn";

alter table "Messages"
    rename column "ProducerServerResponseOutTs" to "ProducerServerResponseOut";


ALTER TABLE "Messages"
    ALTER COLUMN "ConsumerServerRequestIn" TYPE timestamp without time zone
        USING TO_TIMESTAMP("ConsumerServerRequestIn"::double precision / 1000);

ALTER TABLE "Messages"
    ALTER COLUMN "ConsumerServerRequestOut" TYPE timestamp without time zone
        USING TO_TIMESTAMP("ConsumerServerRequestOut"::double precision / 1000);

ALTER TABLE "Messages"
    ALTER COLUMN "ConsumerServerResponseIn" TYPE timestamp without time zone
        USING TO_TIMESTAMP("ConsumerServerResponseIn"::double precision / 1000);

ALTER TABLE "Messages"
    ALTER COLUMN "ConsumerServerResponseOut" TYPE timestamp without time zone
        USING TO_TIMESTAMP("ConsumerServerResponseOut"::double precision / 1000);

ALTER TABLE "Messages"
    ALTER COLUMN "ProducerServerRequestIn" TYPE timestamp without time zone
        USING TO_TIMESTAMP("ProducerServerRequestIn"::double precision / 1000);

ALTER TABLE "Messages"
    ALTER COLUMN "ProducerServerRequestOut" TYPE timestamp without time zone
        USING TO_TIMESTAMP("ProducerServerRequestOut"::double precision / 1000);

ALTER TABLE "Messages"
    ALTER COLUMN "ProducerServerResponseIn" TYPE timestamp without time zone
        USING TO_TIMESTAMP("ProducerServerResponseIn"::double precision / 1000);

ALTER TABLE "Messages"
    ALTER COLUMN "ProducerServerResponseOut" TYPE timestamp without time zone
        USING TO_TIMESTAMP("ProducerServerResponseOut"::double precision / 1000);


